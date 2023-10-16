using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldDebugger : MonoBehaviour
{
    public bool enable;
    
    [Header("Entity 1 Parameters")]
    public float ent1TargetAngle;
    public float ent1RelativeBearing;

    [Header("Entity 2 Parameters")]
    public float ent2TargetAngle;
    public float ent2RelativeBearing;

    [Header("Distance")]
    public float distance;
    public float TCPA;

    [Header("Changeable Variables")]
    public float newEnt1XOffset;
    public float newEnt1YOffset;
    public float newEnt2XOffset;
    public float newEnt2YOffset;

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
        if(enable)
        {
            Potential p1 = DistanceMgr.inst.GetPotential(EntityMgr.inst.entities[0], EntityMgr.inst.entities[1]);
            Potential p2 = DistanceMgr.inst.GetPotential(EntityMgr.inst.entities[1], EntityMgr.inst.entities[0]);

            ent1TargetAngle = p1.targetAngle;
            ent1RelativeBearing = p1.relativeBearingDegrees;

            ent2TargetAngle = p2.targetAngle;
            ent2RelativeBearing = p2.relativeBearingDegrees;

            distance = (EntityMgr.inst.entities[0].position - EntityMgr.inst.entities[1].position).magnitude;
            TCPA = p1.cpaInfo.time;

            EntityMgr.inst.entities[0].xOffset = newEnt1XOffset;
            EntityMgr.inst.entities[0].yOffset = newEnt1YOffset;
            EntityMgr.inst.entities[1].xOffset = newEnt2XOffset;
            EntityMgr.inst.entities[1].yOffset = newEnt2YOffset;
        }
    }
}
