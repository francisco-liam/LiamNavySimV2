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
    public Vector3 front = Vector3.zero;

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
    public float attractionCoefficient = 22500;
    public float attractiveExponent = -1;
    public int numFields = 5;

    public int numBackFields;
    public int numFrontFields;
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
        front = transform.Find("Front").transform.position;
        if(entitySize == EntitySize.Small)
        {
            numBackFields = 0;
            numFrontFields = 0;
        }
        else if(entitySize == EntitySize.Medium)
        {
            numBackFields = 1;
            numFrontFields = 1;
        }
        else
        {
            numBackFields = 2;
            numFrontFields = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
