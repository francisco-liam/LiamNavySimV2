using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Follow : Move
{
    
    public Vector3 relativeOffset;
    public Follow(Entity381 ent, Entity381 target, Vector3 delta): base(ent, target.transform.position)
    {
        targetEntity = target;
        relativeOffset = delta;
    }

    // Start is called before the first frame update
    public override void Init()
    {
        //Debug.Log("Follow:\t Following: " + targetEntity.gameObject.name);
        offset = targetEntity.transform.TransformVector(relativeOffset);
        line = LineMgr.inst.CreateFollowLine(entity.position, targetEntity.position + offset, targetEntity.position);
        potentialLine = LineMgr.inst.CreatePotentialLine(entity.position);
        line.gameObject.SetActive(false);
    }

    public float followThreshold = 10000;
    public Vector3 offset;
    // Update is called once per frame
    public override void Tick()
    {
        offset = targetEntity.transform.TransformVector(relativeOffset);
        movePosition = targetEntity.transform.position + offset;
        base.Tick();
        //entity.desiredHeading = ComputePredictiveDH(relativeOffset);
        if (diff.sqrMagnitude < followThreshold)
        {
            entity.desiredSpeed = targetEntity.speed;
            //entity.desiredHeading = targetEntity.heading;
            entity.minSpeed = 0;
        }
        else if (diff.magnitude < entity.potentialDistanceThreshold * 15f)
        {
            if (targetEntity.entitySize == EntitySize.Large)
            {
                entity.minSpeed = targetEntity.speed * 1.15f;
                if (entity.showPot)
                    Debug.Log("large");
            }
                
            if (targetEntity.entitySize == EntitySize.Medium)
            {
                entity.minSpeed = targetEntity.speed * 1.22f;
                if (entity.showPot)
                    Debug.Log("medium");
            }
                
        }
        else
        {
            entity.minSpeed = 0;
            Debug.Log("else");
        }
            

        if (entity.showPot)
        {
            //Debug.Log(diff.sqrMagnitude + " " + followThreshold);
        }
        

    }

    public bool done = false;//user can set it to done

    public override bool IsDone()
    {
        return done;
    }

    public override void Stop()
    {
        base.Stop();
        entity.desiredSpeed = 0;
        LineMgr.inst.DestroyLR(line);
        LineMgr.inst.DestroyLR(potentialLine);
    }

    Vector3 relativeVelocity;
    public float predictedInterceptTime;
    public Vector3 predictedMovePosition;
    Vector3 predictedDiff;
    //------------------------------------------------------
    public float ComputePredictiveDH(Vector3 relativeOffset)
    {
        float dh;
        movePosition = targetEntity.position + targetEntity.transform.TransformVector(relativeOffset);
        diff = movePosition - entity.position; 
        relativeVelocity = entity.velocity - targetEntity.velocity;
        predictedInterceptTime = diff.magnitude / relativeVelocity.magnitude;
        if (predictedInterceptTime >= 0) {
            predictedMovePosition = movePosition + (targetEntity.velocity * predictedInterceptTime);
            predictedDiff = predictedMovePosition - entity.position;
            dh = Utils.Degrees360(Mathf.Atan2(predictedDiff.x, predictedDiff.z) * Mathf.Rad2Deg);
        } else {
            dh = ComputePotentialDHDS().dh;
        }
        return dh;
    }

}
