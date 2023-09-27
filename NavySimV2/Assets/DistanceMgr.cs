using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Potential
{
    public Entity381 ownship;
    public Entity381 target;

    public List<float> distance = new List<float>(new float[15]);
    //
    public float frontDistance;
    public float backDistance;
    //
    public List<Vector3> diff = new List<Vector3>(new Vector3[15]);
    //
    public Vector3 frontDiff;
    public Vector3 backDiff;
    //
    public Vector3 relativeVelocity; //Your vel relative to me (yourVel - myVel)
    public List<Vector3> direction = new List<Vector3>(new Vector3[15]); //normalized diff
    //
    public Vector3 frontDirection;
    public Vector3 backDirection;
    //
    public float relativeBearingDegrees;
    public CPAInfo cpaInfo;
    public float targetAngle;

    public Potential(Entity381 own, Entity381 tgt)
    {
        ownship = own;
        target = tgt;
        cpaInfo = new CPAInfo(own, target);

    }
    void InitDefaults()
    {
        for(int i =0; i < diff.Count; i++)
        {
            distance[i] = 0;
            diff[i] = Vector3.zero;
            direction[i] = Vector3.zero;
        }
        relativeVelocity = Vector3.zero;
        relativeBearingDegrees = 0;
        cpaInfo = new CPAInfo(ownship, target);
        targetAngle = 0;
    }
}

[System.Serializable]
public class CPAInfo
{
    public Entity381 ownship;
    public Entity381 target;
    public Vector3 ownShipPosition = Vector3.zero;
    public Vector3 targetPosition = Vector3.zero;
    public float time = 0;
    public float range = 0;
    public float targetRelativeBearing = 0;
    public float targetAbsBearing = 0;
    public float targetAngle;
    public Vector3 relativeVelocity = Vector3.zero;

    Vector3 velDiff = Vector3.zero;
    Vector3 posDiff = Vector3.zero;
    float relSpeedSquared = 0;
    Vector3 diff;

    public CPAInfo(Entity381 e1, Entity381 e2)
    {
        ownship = e1;
        target = e2;
    }

    public void ReCompute()
    {
        velDiff = ownship.velocity - target.velocity;
        posDiff = ownship.position - target.position;
        relativeVelocity = target.velocity - ownship.velocity;
        relSpeedSquared = Vector3.Dot(velDiff, velDiff);
        if (relSpeedSquared < Utils.EPSILON * 10)
            time = 0;
        else
            time = -Vector3.Dot(posDiff, velDiff) / relSpeedSquared;
        if (time < 0) time = 0;
        ownShipPosition = ownship.position + ownship.velocity * time;
        targetPosition = target.position + target.velocity * time;
        range = Vector3.Distance(ownShipPosition, targetPosition);

        diff = targetPosition - ownShipPosition;
        targetAbsBearing = Utils.Degrees360(Utils.VectorToHeadingDegrees(diff));
        targetRelativeBearing = Utils.Degrees360(Utils.AngleDiffPosNeg(targetAbsBearing, ownship.heading));
        targetAngle = Utils.Degrees360(targetAbsBearing + 180 - target.heading);

    }
};

public class DistanceMgr : MonoBehaviour
{
    public static DistanceMgr inst;
    private void Awake()
    {
        inst = this;
    }

    public Potential[,] potentials2D;
    public Dictionary<Entity381, Dictionary<Entity381, Potential>> potentialsDictionary;
    public List<List<Potential>> potentialsList;

    // Start is called before the first frame update
    void Start()
    {

    }

    public bool isInitialized = false;
    public int i = 0;
    public int j = 0;
    public int index = 0;
    public void Initialize()
    {
        isInitialized = true;
        potentialsDictionary = new Dictionary<Entity381, Dictionary<Entity381, Potential>>();
        potentialsList = new List<List<Potential>>();
        int n = EntityMgr.inst.entities.Count;
        potentials2D = new Potential[n, n];
        i = 0;
        foreach (Entity381 ent1 in EntityMgr.inst.entities)
        {
            Dictionary<Entity381, Potential> ent1PotDictionary = new Dictionary<Entity381, Potential>();
            List<Potential> ent1PotList = new List<Potential>();
            potentialsDictionary.Add(ent1, ent1PotDictionary);
            potentialsList.Add(ent1PotList);
            j = 0;
            foreach (Entity381 ent2 in EntityMgr.inst.entities)
            {
                Potential pot = new Potential(ent1, ent2);
                ent1PotDictionary.Add(ent2, pot);
                ent1PotList.Add(pot);
                potentials2D[i, j] = pot;
                j++;
            }
            i++;
        }
        index = 0;
    }

    void Stop()
    {
        isInitialized = false;
    }
    // Update is called once per frame

    public int fraction;

    void Update()
    {
        /*
        if (isInitialized)
        {
            for (int k = 0; k < i/(fraction); k++)
            {
                UpdatePotentials();
                index = (index + 1) % i;
            }
        }
        else
            Initialize();
        */
        
    }

    public List<Potential> selectedEntityPotentials; // For debugging
    void UpdatePotentials()
    {
        Potential p1, p2;
        Entity381 ent1, ent2;
        //for (int i = 0; i < EntityMgr.inst.entities.Count - 1; i++)
        //{
            int i = index;
            
            ent1 = EntityMgr.inst.entities[i];
            if (ent1 == SelectionMgr.inst.selectedEntity)
                selectedEntityPotentials = potentialsList[i];
            //don't do diagonal
            for (int j = i + 1; j < EntityMgr.inst.entities.Count; j++)
            {
                ent2 = EntityMgr.inst.entities[j];

                p1 = potentials2D[i, j];
                p2 = potentials2D[j, i];

                //p1.target.gameObject.GetComponentInChildren<PotentialField>();
                
                Vector3 p1ShiftVec = new Vector3();
                p1ShiftVec.x = (Mathf.Sin(p1.target.heading * Mathf.Deg2Rad));
                p1ShiftVec.y = 0;
                p1ShiftVec.z = (Mathf.Cos(p1.target.heading * Mathf.Deg2Rad));

                Vector3 p1ShiftVecRight = new Vector3(p1ShiftVec.z, 0, -p1ShiftVec.x);
                Vector3 p1ShiftVecLeft = -p1ShiftVecRight;

                Vector3 p2ShiftVec = new Vector3();
                p2ShiftVec.x = (Mathf.Sin(p2.target.heading * Mathf.Deg2Rad));
                p2ShiftVec.y = 0;
                p2ShiftVec.z = (Mathf.Cos(p2.target.heading * Mathf.Deg2Rad));

                Vector3 p2ShiftVecRight = new Vector3(p2ShiftVec.z, 0, -p2ShiftVec.x);
                Vector3 p2ShiftVecLeft = -p2ShiftVecRight;

                

                //p1
                for (int k = 0; k < p1.target.numFields; k++)
                {
                    if(p1.target.numFields > 1)
                        p1.diff[k] = ((p1.target.position - p1ShiftVec.normalized * p1.target.length / 2) + (k / (p1.target.numFields - 1.0f) * p1ShiftVec.normalized * p1.target.length)) - (p1.ownship.position + (p2ShiftVec.normalized * p1.ownship.length / 2));
                    else
                        p1.diff[k] = p1.target.position - p1.ownship.position;
                    p1.distance[k] = p1.diff[k].magnitude;
                    p1.direction[k] = p1.diff[k].normalized;
                }
                p1.cpaInfo.ReCompute();
                p1.relativeVelocity = p1.cpaInfo.relativeVelocity;
                p1.targetAngle = p1.cpaInfo.targetAngle;
                p1.relativeBearingDegrees = p1.cpaInfo.targetRelativeBearing;

                p1.frontDiff = (p1.target.position + (p1ShiftVec.normalized * p1.target.length / 2) + (p1ShiftVec.normalized * 20) + (p1ShiftVecRight.normalized*20)) - (p1.ownship.position + (p2ShiftVec.normalized * p1.ownship.length / 2));
                p1.frontDistance = p1.frontDiff.magnitude;
                p1.frontDirection = p1.frontDiff.normalized;

                p1.backDiff = (p1.target.position - (p1ShiftVec.normalized * p1.target.length / 2) - (p1ShiftVec.normalized * 20) + (p1ShiftVecLeft.normalized * 20)) - (p1.ownship.position + (p2ShiftVec.normalized * p1.ownship.length / 2));
                p1.backDistance = p1.backDiff.magnitude;
                p1.backDirection = p1.backDiff.normalized;


                //p2
                for (int k = 0; k < p2.target.numFields; k++)
                {
                    if (p2.target.numFields > 1)
                        p2.diff[k] = ((p2.target.position - p2ShiftVec.normalized * p2.target.length / 2) + (k / ((p2.target.numFields - 1.0f)) * p2ShiftVec.normalized * p2.target.length)) - (p2.ownship.position + (p1ShiftVec.normalized * p2.ownship.length / 2));
                    else
                        p2.diff[k] = p2.target.position - p2.ownship.position;
                    p2.distance[k] = p2.diff[k].magnitude;
                    p2.direction[k] = p2.diff[k].normalized;
                }
                p2.cpaInfo.ReCompute();
                p2.relativeVelocity = p2.cpaInfo.relativeVelocity;
                p2.targetAngle = p2.cpaInfo.targetAngle;
                p2.relativeBearingDegrees = p2.cpaInfo.targetRelativeBearing;

                p2.frontDiff = (p2.target.position + (p2ShiftVec.normalized * p2.target.length / 2) + (p2ShiftVec.normalized * 20) + (p2ShiftVecRight.normalized * 20)) - (p2.ownship.position + (p1ShiftVec.normalized * p2.ownship.length / 2));
                p2.frontDistance = p2.frontDiff.magnitude;
                p2.frontDirection = p2.frontDiff.normalized;

                p2.backDiff = (p2.target.position - (p2ShiftVec.normalized * p2.target.length / 2) - (p2ShiftVec.normalized * 20) + (p2ShiftVecLeft.normalized * 20)) - (p2.ownship.position + (p1ShiftVec.normalized * p2.ownship.length / 2));
                p2.backDistance = p2.backDiff.magnitude;
                p2.backDirection = p2.backDiff.normalized;
        }
        //}
    }

    public Potential GetPotential(Entity381 e1, Entity381 e2)
    {
        Potential p = null;
        if (isInitialized)
            p = potentialsDictionary[e1][e2];
        return p;
    }

}
