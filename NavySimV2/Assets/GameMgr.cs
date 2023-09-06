﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    public static GameMgr inst;
    private void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = Vector3.zero;
        foreach(GameObject go in EntityMgr.inst.entityPrefabs) {
            Entity381 ent = EntityMgr.inst.CreateEntity(go.GetComponent<Entity381>().entityType, position, Vector3.zero);
            position.x += 200;
        }
    }

    [Header("Pilot Vessel Spawn Variables")]
    public Vector3 position;
    public float spread = 20;
    public float colNum = 10;
    // Update is called once per frame

    [Header("Test Case 1 Variables")]
    public EntityType entityType0;
    public EntityType entityType1;
    public EntityType entityType2;

    [Header("Test Case 2-4 Object")]
    public GameObject testCases;

    void Update()
    {

        //spawns 100 pilot vessels

        if (Input.GetKeyUp(KeyCode.F12)) {
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    Entity381 ent = EntityMgr.inst.CreateEntity(EntityType.PilotVessel, position, Vector3.zero);
                    position.z += spread;
                }
                position.x += spread;
                position.z = 0;
            }
            DistanceMgr.inst.Initialize();
        }

        if(Input.GetKeyUp(KeyCode.F1)) {
            //TestCases.inst.TestCase10();
        }
    }

    public void EnableTestCases()
    {
        testCases.GetComponent<TestCases>().enabled = true;
    }
}
