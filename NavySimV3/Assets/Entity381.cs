using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EntityType
{
    DDG51,
    Container,
    MineSweeper,
    OilServiceVessel,
    OrientExplorer,
    PilotVessel,
    SmitHouston,
    Tanker,
    TugBoat
}

public enum EntitySize
{
    Small = 1,
    Medium = 2,
    Large = 3
}


public class Entity381 : MonoBehaviour
{
    //------------------------------
    // values that change while running
    //------------------------------
    [Header("Changing Values")]
    public bool isSelected = false;
    public bool underway = false;
    public Vector3 position = Vector3.zero;
    public Vector3 velocity = Vector3.zero;

    public float speed;
    public float desiredSpeed;
    public float heading; //degrees
    public float desiredHeading; //degrees
    //------------------------------
    // values that do not change
    //------------------------------
    [Header("Constant Values")]
    public float acceleration;
    public float turnRate;
    public float maxSpeed;
    public float minSpeed;
    public float mass;
    public float length;

    //------------------------------
    // potential field variables
    //------------------------------
    [Header("Potential Field Variables")]
    public float potentialDistanceThreshold = 500;
    public float repulsiveCoefficient = 1000;
    public float repulsiveExponent = -2.0f;
    public float taCoefficient;
    public float taExponent;
    public float rbCoefficient;
    public float rbExponent;
    public float attractionCoefficient = 22500;
    public float attractiveExponent = -1;
    public int numFields = 5;
    public Vector3[] fieldPos;
    public float xOffset;
    public float yOffset;
    public Vector3 front;
    public bool showPot = false;

    public EntitySize entitySize;

    public EntityType entityType;

    [Header("Camera and Selection")]
    public GameObject cameraRig;
    public GameObject selectionCircle;

    // Start is called before the first frame update
    void Start()
    {
        cameraRig = transform.Find("CameraRig").gameObject;
        selectionCircle = transform.Find("Decorations").Find("SelectionCylinder").gameObject;

        fieldPos = new Vector3[numFields];
        numFields = 1;

        Vector3 shiftVec = new Vector3();
        shiftVec.x = (Mathf.Sin(heading * Mathf.Deg2Rad));
        shiftVec.y = 0;
        shiftVec.z = (Mathf.Cos(heading * Mathf.Deg2Rad));
        Vector3 shiftVecRight = new Vector3(shiftVec.z, 0, -shiftVec.x);

        fieldPos[0] = position + shiftVec * yOffset + shiftVecRight * xOffset;
        front = position + (shiftVec.normalized * length / 2);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 shiftVec = new Vector3();
        shiftVec.x = (Mathf.Sin(heading * Mathf.Deg2Rad));
        shiftVec.y = 0;
        shiftVec.z = (Mathf.Cos(heading * Mathf.Deg2Rad));
        Vector3 shiftVecRight = new Vector3(shiftVec.z, 0, -shiftVec.x);

        fieldPos[0] = position + shiftVec * yOffset + shiftVecRight * xOffset;
        front = position + (shiftVec.normalized * length / 2);

        position = transform.localPosition;
        heading = transform.localEulerAngles.y;
    }
}
