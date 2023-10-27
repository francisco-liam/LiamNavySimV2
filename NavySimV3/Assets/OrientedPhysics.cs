using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientedPhysics : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponentInParent<Entity381>();
        entity.position = transform.localPosition;
    }

    public Entity381 entity;


    // Update is called once per frame
    void Update()
    {
        if(Utils.ApproximatelyEqual(entity.speed, entity.desiredSpeed)) {
            ;
        } else if(entity.speed < entity.desiredSpeed) {
            entity.speed = entity.speed + entity.acceleration * Time.deltaTime * ControlMgr.inst.GameSpeed;
        } else if (entity.speed > entity.desiredSpeed) {
            entity.speed = entity.speed - entity.acceleration * Time.deltaTime * ControlMgr.inst.GameSpeed;
        }
        entity.speed = Utils.Clamp(entity.speed, entity.minSpeed, entity.maxSpeed);

        //heading
        eulerRotation = transform.localEulerAngles;
        if (Utils.ApproximatelyEqual(entity.heading, entity.desiredHeading)) {
            ;
        } else if (Utils.AngleDiffPosNeg(entity.desiredHeading, entity.heading) > 0) {
            eulerRotation.y += entity.turnRate * Time.deltaTime * ControlMgr.inst.GameSpeed;
        } else if (Utils.AngleDiffPosNeg(entity.desiredHeading, entity.heading) < 0) {
            eulerRotation.y -= entity.turnRate * Time.deltaTime * ControlMgr.inst.GameSpeed;
        }
        eulerRotation.y = Utils.Degrees360(eulerRotation.y);
        entity.heading = eulerRotation.y;
        //
        entity.velocity.x = Mathf.Sin(entity.heading * Mathf.Deg2Rad) * entity.speed;
        entity.velocity.y = 0;
        entity.velocity.z = Mathf.Cos(entity.heading * Mathf.Deg2Rad) * entity.speed;

        entity.transform.position = entity.transform.position + entity.velocity * Time.deltaTime * ControlMgr.inst.GameSpeed;
        entity.position = transform.localPosition;

        transform.localEulerAngles = eulerRotation;

        if(entity.speed > 0)
            entity.underway = true;
        else
            entity.underway = false;

    }

    public Vector3 eulerRotation = Vector3.zero;


}
