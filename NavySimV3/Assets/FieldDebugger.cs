using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldDebugger : MonoBehaviour
{
    public bool enableParametersChange;
    public bool enablePosAndHeadingChange;

    [Header("Entity 1 Parameters")]
    public float ent1TargetAngle;
    public float ent1RelativeBearing;

    [Header("Entity 2 Parameters")]
    public float ent2TargetAngle;
    public float ent2RelativeBearing;

    [Header("Distance")]
    public float distance;
    public float TCPA;

    [Header("Offset")]
    [Header("Changeable Variables")]
    public float Ent1XOffset;
    public float Ent1YOffset;
    public float Ent2XOffset;
    public float Ent2YOffset;

    [Header("Entity 1")]
    [Header("Parameters")]
    public float Ent1RepulsiveCoefficient;
    public float Ent1RepulsiveExponent;
    public float Ent1TargetAngleCoefficient;
    public float Ent1TargetAngleExponent;
    public float Ent1RelativeBearingCoefficient;
    public float Ent1RelativeBearingExponent;

    [Header("Entity 2")]
    public float Ent2RepulsiveCoefficient;
    public float Ent2RepulsiveExponent;
    public float Ent2TargetAngleCoefficient;
    public float Ent2TargetAngleExponent;
    public float Ent2RelativeBearingCoefficient;
    public float Ent2RelativeBearingExponent;

    [Header("Entity 1")]
    [Header("Position and Heading")]
    public Vector3 Ent1Position;
    public float Ent1Heading;

    [Header("Entity 2")]
    public Vector3 Ent2Position;
    public float Ent2Heading;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
#if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
#endif
    }

    // Update is called once per frame
    void Update()
    {

        Potential p1 = DistanceMgr.inst.GetPotential(EntityMgr.inst.entities[0], EntityMgr.inst.entities[1]);
        Potential p2 = DistanceMgr.inst.GetPotential(EntityMgr.inst.entities[1], EntityMgr.inst.entities[0]);

        ent1TargetAngle = p1.targetAngle;
        ent1RelativeBearing = p1.relativeBearingDegrees;

        ent2TargetAngle = p2.targetAngle;
        ent2RelativeBearing = p2.relativeBearingDegrees;

        distance = (EntityMgr.inst.entities[0].position - EntityMgr.inst.entities[1].position).magnitude;
        TCPA = p1.cpaInfo.time;
        if (enableParametersChange)
        {
            EntityMgr.inst.entities[0].xOffset = Ent1XOffset;
            EntityMgr.inst.entities[0].yOffset = Ent1YOffset;
            EntityMgr.inst.entities[0].repulsiveCoefficient = Ent1RepulsiveCoefficient;
            EntityMgr.inst.entities[0].repulsiveExponent = Ent1RepulsiveExponent;
            EntityMgr.inst.entities[0].taCoefficient = Ent1TargetAngleCoefficient;
            EntityMgr.inst.entities[0].taExponent = Ent1TargetAngleExponent;
            EntityMgr.inst.entities[0].rbCoefficient = Ent1RelativeBearingCoefficient;
            EntityMgr.inst.entities[0].rbExponent = Ent1RelativeBearingExponent;

            EntityMgr.inst.entities[1].xOffset = Ent2XOffset;
            EntityMgr.inst.entities[1].yOffset = Ent2YOffset;
            EntityMgr.inst.entities[1].repulsiveCoefficient = Ent2RepulsiveCoefficient;
            EntityMgr.inst.entities[1].repulsiveExponent = Ent2RepulsiveExponent;
            EntityMgr.inst.entities[1].taCoefficient = Ent2TargetAngleCoefficient;
            EntityMgr.inst.entities[1].taExponent = Ent2TargetAngleExponent;
            EntityMgr.inst.entities[1].rbCoefficient = Ent2RelativeBearingCoefficient;
            EntityMgr.inst.entities[1].rbExponent = Ent2RelativeBearingExponent;
        }
        else
        {
            Ent1XOffset = EntityMgr.inst.entities[0].xOffset;
            Ent1YOffset = EntityMgr.inst.entities[0].yOffset;
            Ent1RepulsiveCoefficient = EntityMgr.inst.entities[0].repulsiveCoefficient;
            Ent1RepulsiveExponent = EntityMgr.inst.entities[0].repulsiveExponent;
            Ent1TargetAngleCoefficient = EntityMgr.inst.entities[0].taCoefficient;
            Ent1TargetAngleExponent = EntityMgr.inst.entities[0].taExponent;
            Ent1RelativeBearingCoefficient = EntityMgr.inst.entities[0].rbCoefficient;
            Ent1RelativeBearingExponent = EntityMgr.inst.entities[0].rbExponent;

            Ent2XOffset = EntityMgr.inst.entities[1].xOffset;
            Ent2YOffset = EntityMgr.inst.entities[1].yOffset;
            Ent2RepulsiveCoefficient = EntityMgr.inst.entities[1].repulsiveCoefficient;
            Ent2RepulsiveExponent = EntityMgr.inst.entities[1].repulsiveExponent;
            Ent2TargetAngleCoefficient = EntityMgr.inst.entities[1].taCoefficient;
            Ent2TargetAngleExponent = EntityMgr.inst.entities[1].taExponent;
            Ent2RelativeBearingCoefficient = EntityMgr.inst.entities[1].rbCoefficient;
            Ent2RelativeBearingExponent = EntityMgr.inst.entities[1].rbExponent;
        }

        if (enablePosAndHeadingChange)
        {
            EntityMgr.inst.entities[0].position = Ent1Position;
            EntityMgr.inst.entities[0].heading = Ent1Heading;

            EntityMgr.inst.entities[1].position = Ent2Position;
            EntityMgr.inst.entities[1].heading = Ent2Heading;
        }
        else
        {
            Ent1Position = EntityMgr.inst.entities[0].position;
            Ent1Heading = EntityMgr.inst.entities[0].heading;

            Ent2Position = EntityMgr.inst.entities[1].position;
            Ent2Heading = EntityMgr.inst.entities[1].heading;
        }
    }
}
