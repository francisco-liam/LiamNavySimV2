using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Move : Command
{
    public Move(Entity381 ent, Vector3 pos) : base(ent)
    {
        movePosition = pos;
    }

    public LineRenderer potentialLine;
    public override void Init()
    {
        //Debug.Log("MoveInit:\tMoving to: " + movePosition);
        line = LineMgr.inst.CreateMoveLine(entity.position, movePosition);
        line.gameObject.SetActive(false);
        potentialLine = LineMgr.inst.CreatePotentialLine(entity.position);
        line.gameObject.SetActive(true);
    }

    public override void Tick()
    {
        DHDS dhds;
        if (AIMgr.inst.isPotentialFieldsMovement)
            dhds = ComputePotentialDHDS();
        else
            dhds = ComputeDHDS();

        entity.desiredHeading = dhds.dh;
        entity.desiredSpeed = dhds.ds;
        line.SetPosition(1, movePosition);
    }

    public Vector3 diff = Vector3.positiveInfinity;
    public float dhRadians;
    public float dhDegrees;
    public DHDS ComputeDHDS()
    {
        diff = movePosition - entity.position;
        dhRadians = Mathf.Atan2(diff.x, diff.z);
        dhDegrees = Utils.Degrees360(Mathf.Rad2Deg * dhRadians);
        return new DHDS(dhDegrees, entity.maxSpeed);

    }

    public DHDS ComputePotentialDHDS()
    {
        diff = movePosition - entity.position;
        Potential p;
        repulsivePotential = Vector3.zero;
        foreach (Entity381 ent in EntityMgr.inst.entities)
        {
            if (ent == entity) continue;
            p = DistanceMgr.inst.GetPotential(entity, ent);
            for(int i =0; i < p.target.numFields; i++)
            {
                bool add = true;
                UnitAI targetAi = p.target.gameObject.GetComponent<UnitAI>();

                if (targetAi.commands.Count > 0 && targetAi.commands[0] is Follow && targetAi.commands[0].targetEntity == entity)
                    add = false;
                
                if (!p.target.underway && add)
                {
                    if (p.distance[i] < p.ownship.potentialDistanceThreshold)
                    {
                        repulsivePotential += p.direction[i] * p.target.mass / 40 * p.target.length / 20 *
                            p.target.repulsiveCoefficient / p.target.numFields * Mathf.Pow(p.diff[i].magnitude, p.target.repulsiveExponent);
                    }
                }
                else
                {
                    if (p.distance[i] < p.ownship.potentialDistanceThreshold && add)
                    {
                        if (i != p.target.numFields - 1)
                        {
                            repulsivePotential += p.direction[i] * p.target.mass / 40 * p.target.length / 20 *
                                p.target.repulsiveCoefficient / p.target.numFields * Mathf.Pow(p.diff[i].magnitude, p.target.repulsiveExponent);
                        }
                        else
                        {
                            repulsivePotential += p.direction[i] * p.target.mass / 40 * p.target.length / 20 *
                                p.target.repulsiveCoefficient / 2 * Mathf.Pow(p.diff[i].magnitude, p.target.repulsiveExponent);
                        }
                    }
                }
            }

            if(p.target.entitySize != EntitySize.Small)
            {
                if (p.frontDistance < p.ownship.potentialDistanceThreshold)
                {
                    repulsivePotential += p.frontDirection * p.target.mass / 40 * p.target.length / 100 *
                                p.target.repulsiveCoefficient / p.target.numFields * Mathf.Pow(p.frontDiff.magnitude, p.target.repulsiveExponent);
                }

                if (p.backDistance < p.ownship.potentialDistanceThreshold)
                {
                    repulsivePotential += p.backDirection * p.target.mass / 40 * p.target.length / 100 *
                                p.target.repulsiveCoefficient / p.target.numFields * Mathf.Pow(p.backDiff.magnitude, p.target.repulsiveExponent);
                }
            }
            
            //repulsivePotential += p.diff;
        }
        //repulsivePotential *= repulsiveCoefficient * Mathf.Pow(repulsivePotential.magnitude, repulsiveExponent);
        attractivePotential = movePosition - entity.position;
        Vector3 tmp = attractivePotential.normalized;
        attractivePotential = tmp *
            entity.attractionCoefficient * Mathf.Pow(attractivePotential.magnitude, entity.attractiveExponent);
        potentialSum = attractivePotential - repulsivePotential;

        Vector3 distance = movePosition - entity.position;
        float angDiff = Vector3.Angle(repulsivePotential, attractivePotential);
        //if (entity.showPot)
            //Debug.Log(distance.magnitude);
        

        dh = Utils.Degrees360(Mathf.Rad2Deg * Mathf.Atan2(potentialSum.x, potentialSum.z));

        angleDiff = Utils.Degrees360(Utils.AngleDiffPosNeg(dh, entity.heading));
        cosValue = (Mathf.Cos(angleDiff * Mathf.Deg2Rad) + 1) / 2.0f; // makes it between 0 and 1
        ds = entity.maxSpeed * cosValue;

        return new DHDS(dh, ds);
    }
    public Vector3 attractivePotential = Vector3.zero;
    public Vector3 potentialSum = Vector3.zero;
    public Vector3 repulsivePotential = Vector3.zero;
    public float dh;
    public float angleDiff;
    public float cosValue;
    public float ds;



    public float doneDistanceSq = 1000;
    public override bool IsDone()
    {

        return ((entity.position - movePosition).sqrMagnitude < doneDistanceSq);
    }

    public override void Stop()
    {
        entity.desiredSpeed = 0;
        LineMgr.inst.DestroyLR(line);
        LineMgr.inst.DestroyLR(potentialLine);
    }
}
