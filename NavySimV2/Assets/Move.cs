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
        repulsivePotential = Vector3.zero;
        int layerMask = 1 << 10;
        Collider[] colliders = Physics.OverlapSphere(entity.front, entity.potentialDistanceThreshold, layerMask);
        fieldList = new List<Collider>(colliders);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<PotentialField>().entity == entity)
                fieldList.Remove(collider);
        }
        foreach (Collider collider in fieldList)
        {
            PotentialField f = collider.gameObject.GetComponent<PotentialField>();
            Entity381 target = f.entity;
            UnitAI targetAi= target.gameObject.GetComponent<UnitAI>();
            bool add = true;
            Vector3 fieldDiff = f.transform.position - entity.front;
            Vector3 direction = fieldDiff.normalized;

            if (targetAi.commands.Count > 0 && targetAi.commands[0] is Follow && targetAi.commands[0].targetEntity == entity)
                add = false;

            if(add)
                repulsivePotential += direction * target.mass / 40 * target.length / 20 *
                          f.repulsiveCoefficient / target.numFields * Mathf.Pow(fieldDiff.magnitude, f.repulsiveExponent);
        }
        //repulsivePotential *= repulsiveCoefficient * Mathf.Pow(repulsivePotential.magnitude, repulsiveExponent);
        attractivePotential = movePosition - entity.position;
        Vector3 tmp = attractivePotential.normalized;
        attractivePotential = tmp *
            entity.attractionCoefficient * Mathf.Pow(attractivePotential.magnitude, entity.attractiveExponent);
        potentialSum = attractivePotential - repulsivePotential;

        Vector3 distance = movePosition - entity.position;
        float angDiff = Vector3.Angle(repulsivePotential, attractivePotential);        

        dh = Utils.Degrees360(Mathf.Rad2Deg * Mathf.Atan2(potentialSum.x, potentialSum.z));

        angleDiff = Utils.Degrees360(Utils.AngleDiffPosNeg(dh, entity.heading));
        cosValue = (Mathf.Cos(angleDiff * Mathf.Deg2Rad) + 1) / 2.0f; // makes it between 0 and 1
        ds = entity.maxSpeed * cosValue;

        return new DHDS(dh, ds);
    }
    public List<Collider> fieldList;
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
