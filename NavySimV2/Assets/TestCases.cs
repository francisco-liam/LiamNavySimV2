using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Transactions;
using UnityEngine;

public enum TestCase
{
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten
}

public class TestCases : MonoBehaviour
{
    public static TestCases inst;
    private void Awake()
    {
        inst = this;
    }

    [Header("Test Case 1-4 Variables")]
    public EntityType entityType3;
    public EntityType entityType4;
    public EntityType entityType5;
    public float test2Speed;
    public TestCase testCase;

    [Header("Test Case 5-7 Variables")]
    public EntityType entityType6;
    public EntityType entityType7;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (testCase == TestCase.One)
            TestCase1();
        else if (testCase == TestCase.Two || testCase == TestCase.Three || testCase == TestCase.Four)
            TestCase2to4();
        else if (testCase == TestCase.Five)
            TestCase5();
        else if (testCase == TestCase.Six)
            TestCase6();
        else if (testCase == TestCase.Seven)
            TestCase7();
        else if (testCase == TestCase.Eight)
            TestCase8();
        else if (testCase == TestCase.Nine)
            TestCase9();
        else
            TestCase10();
    }


    void TestCase1()
    {
        GameObject camera = GameObject.Find("YawMoveNode");
        camera.transform.localRotation = Quaternion.identity;
        camera.transform.localPosition = new Vector3(-1500, 0, -1500);
        Entity381 ent0 = EntityMgr.inst.CreateEntity(entityType3, new Vector3(-1500, 0, -1500), Vector3.zero);
        Entity381 ent1 = EntityMgr.inst.CreateEntity(entityType4, new Vector3(-1300, 0, -1500), Vector3.zero);
        Entity381 ent2 = EntityMgr.inst.CreateEntity(entityType5, new Vector3(-1100, 0, -1500), Vector3.zero);
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Entity381 ent3 = EntityMgr.inst.CreateEntity(EntityType.PilotVessel, new Vector3(-1800 + (j * 20), 0, -1500 + (i * 50)), Vector3.zero);
                ent3.gameObject.AddComponent<TestCaseEntity>();
                ent3.GetComponent<TestCaseEntity>().added = false;
            }
        }
        DistanceMgr.inst.Initialize();
    }

    void TestCase2to4()
    {
        GameObject camera = GameObject.Find("YawMoveNode");
        camera.transform.localRotation = Quaternion.identity;
        camera.transform.localPosition = new Vector3(2500, 0, -1500);
        Entity381 ent0 = EntityMgr.inst.CreateEntity(entityType3, new Vector3(2500, 0, -1500), Vector3.zero);
        Entity381 ent1 = EntityMgr.inst.CreateEntity(entityType4, new Vector3(2300, 0, -1500), Vector3.zero);
        Entity381 ent2 = EntityMgr.inst.CreateEntity(entityType5, new Vector3(2100, 0, -1500), Vector3.zero);

        ent0.desiredSpeed = test2Speed;
        ent1.desiredSpeed = test2Speed;
        ent2.desiredSpeed = test2Speed;

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Entity381 ent3 = EntityMgr.inst.CreateEntity(EntityType.PilotVessel, new Vector3(2800 + (j * 20), 0, -1500 + (i * 50)), Vector3.zero);
                ent3.gameObject.AddComponent<TestCaseEntity>();
                ent3.gameObject.GetComponent<TestCaseEntity>().ship1 = ent0;
                ent3.gameObject.GetComponent<TestCaseEntity>().ship2 = ent1;
                ent3.gameObject.GetComponent<TestCaseEntity>().ship3 = ent2;
                ent3.gameObject.GetComponent<TestCaseEntity>().testCase = testCase;
            }
        }
        DistanceMgr.inst.Initialize();
    }

    public void TestCase5()
    {
        GameObject camera = GameObject.Find("YawMoveNode");
        camera.transform.localPosition = new Vector3(2500, 0, 2000);
        camera.transform.localRotation = Quaternion.identity;
        camera.transform.Rotate(0, 270, 0);

        Entity381 ent0 = EntityMgr.inst.CreateEntity(entityType6, new Vector3(2000, 0, 1200), Vector3.zero);
        ent0.desiredHeading = 270f;
        ent0.heading = 270f;
        ent0.gameObject.AddComponent<TestCaseEntity>();
        ent0.GetComponent<TestCaseEntity>().testCase = TestCase.Five;
        ent0.GetComponent<TestCaseEntity>().move = new Vector3(-1000, 0, 1200);
        ent0.GetComponent<TestCaseEntity>().added = false;


        Entity381 ent1 = EntityMgr.inst.CreateEntity(entityType7, new Vector3(1000, 0, 1200), Vector3.zero);
        ent1.desiredHeading = 90f;
        ent1.heading = 90f;
        ent1.gameObject.AddComponent<TestCaseEntity>();
        ent1.GetComponent<TestCaseEntity>().move = new Vector3(4000, 0, 1200);
        ent1.GetComponent<TestCaseEntity>().testCase = TestCase.Five;
        ent1.GetComponent<TestCaseEntity>().added = false;

        DistanceMgr.inst.Initialize();
    }

    public void TestCase6()
    {
        GameObject camera = GameObject.Find("YawMoveNode");
        camera.transform.localPosition = new Vector3(2500, 0, 2000);
        camera.transform.localRotation = Quaternion.identity;
        camera.transform.Rotate(0, 270, 0);

        Entity381 ent0 = EntityMgr.inst.CreateEntity(entityType6, new Vector3(2000, 0, 1200), Vector3.zero);
        ent0.desiredHeading = 270f;
        ent0.heading = 270f;
        ent0.gameObject.AddComponent<TestCaseEntity>();
        ent0.GetComponent<TestCaseEntity>().testCase = TestCase.Six;
        ent0.GetComponent<TestCaseEntity>().move = new Vector3(-10000, 0, 1200);
        ent0.GetComponent<TestCaseEntity>().added = false;


        Entity381 ent1 = EntityMgr.inst.CreateEntity(entityType7, new Vector3(1000, 0, 1200), Vector3.zero);
        ent1.desiredHeading = 270f;
        ent1.heading = 270f;
        ent1.gameObject.AddComponent<TestCaseEntity>();
        ent1.maxSpeed = 3f;
        ent1.GetComponent<TestCaseEntity>().move = new Vector3(-10000, 0, 1200);
        ent1.GetComponent<TestCaseEntity>().testCase = TestCase.Six;
        ent1.GetComponent<TestCaseEntity>().added = false;

        DistanceMgr.inst.Initialize();
    }

    public void TestCase7()
    {
        GameObject camera = GameObject.Find("YawMoveNode");
        camera.transform.localPosition = new Vector3(3500, 0, 2000);
        camera.transform.localRotation = Quaternion.identity;
        camera.transform.Rotate(0, 270, 0);

        Entity381 ent0 = EntityMgr.inst.CreateEntity(entityType6, new Vector3(3000, 0, 1200), Vector3.zero);
        ent0.desiredHeading = 270f;
        ent0.heading = 270f;
        ent0.gameObject.AddComponent<TestCaseEntity>();
        ent0.GetComponent<TestCaseEntity>().testCase = TestCase.Seven;
        ent0.GetComponent<TestCaseEntity>().move = new Vector3(-10000, 0, 1200);
        ent0.GetComponent<TestCaseEntity>().added = false;

        Entity381 ent1 = EntityMgr.inst.CreateEntity(entityType7, new Vector3(2500, 0, 1700), Vector3.zero);
        ent1.desiredHeading = 180f;
        ent1.heading = 180f;
        ent1.gameObject.AddComponent<TestCaseEntity>();
        ent1.GetComponent<TestCaseEntity>().testCase = TestCase.Seven;
        ent1.GetComponent<TestCaseEntity>().move = new Vector3(2500, 0, -10000);
        ent1.GetComponent<TestCaseEntity>().added = false;

        DistanceMgr.inst.Initialize();
    }

    public void TestCase8()
    {
        GameObject camera = GameObject.Find("YawMoveNode");
        camera.transform.localPosition = new Vector3(3500, 0, -1500);
        camera.transform.localRotation = Quaternion.identity;
        camera.transform.Rotate(0, 270, 0);

        float x = 2000;
        for (int i =0; i <5; i++)
        {
            float z = -2250;
            for (int j = 0; j <4; j++)
            {
                Entity381 ent0 = EntityMgr.inst.CreateEntity(entityType6, new Vector3(x, 0, z), Vector3.zero);
                ent0.desiredHeading = 90f;
                ent0.heading = 90f;
                ent0.gameObject.AddComponent<TestCaseEntity>();
                ent0.GetComponent<TestCaseEntity>().testCase = TestCase.Eight;
                ent0.GetComponent<TestCaseEntity>().move = new Vector3(10000, 0, z);
                ent0.GetComponent<TestCaseEntity>().added = false;
                z -= 200;
            }
            x -= 500;
            
        }
        
        Entity381 ent1 = EntityMgr.inst.CreateEntity(entityType7, new Vector3(3000, 0, -2450), Vector3.zero);
        ent1.desiredHeading = 270f;
        ent1.heading = 270f;
        ent1.gameObject.AddComponent<TestCaseEntity>();
        ent1.GetComponent<TestCaseEntity>().testCase = TestCase.Seven;
        ent1.GetComponent<TestCaseEntity>().move = new Vector3(-10000, 0, -2500);
        ent1.GetComponent<TestCaseEntity>().added = false;

        DistanceMgr.inst.Initialize();
        
    }

    public void TestCase9()
    {
        GameObject camera = GameObject.Find("YawMoveNode");
        camera.transform.localPosition = new Vector3(3500, 0, -1500);
        camera.transform.localRotation = Quaternion.identity;
        camera.transform.Rotate(0, 270, 0);

        Entity381 ent1 = EntityMgr.inst.CreateEntity(entityType7, new Vector3(3000, 0, -2450), Vector3.zero);
        ent1.desiredHeading = 270f;
        ent1.heading = 270f;
        ent1.gameObject.AddComponent<TestCaseEntity>();
        ent1.GetComponent<TestCaseEntity>().testCase = TestCase.Seven;
        ent1.GetComponent<TestCaseEntity>().move = new Vector3(-10000, 0, -2500);
        ent1.GetComponent<TestCaseEntity>().added = false;

        float x = 2000;
        for (int i = 0; i < 5; i++)
        {
            float z = -2250;
            for (int j = 0; j < 4; j++)
            {
                Entity381 ent0 = EntityMgr.inst.CreateEntity(entityType6, new Vector3(x, 0, z), Vector3.zero);
                ent0.desiredHeading = 270f;
                ent0.heading = 270f;
                ent0.maxSpeed = ent1.maxSpeed*0.5f;
                ent0.gameObject.AddComponent<TestCaseEntity>();
                ent0.GetComponent<TestCaseEntity>().testCase = TestCase.Eight;
                ent0.GetComponent<TestCaseEntity>().move = new Vector3(-10000, 0, z);
                ent0.GetComponent<TestCaseEntity>().added = false;
                z -= 200;
            }
            x -= 500;

        }

        DistanceMgr.inst.Initialize();

    }

    public void TestCase10()
    {
        GameObject camera = GameObject.Find("YawMoveNode");
        camera.transform.localPosition = new Vector3(2000, 0, 0);
        camera.transform.localRotation = Quaternion.identity;
        camera.transform.Rotate(0, 180, 0);

        Entity381 ent1 = EntityMgr.inst.CreateEntity(entityType7, new Vector3(2000, 0, -1500), Vector3.zero);
        ent1.desiredHeading = 180f;
        ent1.heading = 180f;
        ent1.gameObject.AddComponent<TestCaseEntity>();
        ent1.GetComponent<TestCaseEntity>().testCase = TestCase.Seven;
        ent1.GetComponent<TestCaseEntity>().move = new Vector3(2500, 0, -10000);
        ent1.GetComponent<TestCaseEntity>().added = false;

        float x = 2000;
        for (int i = 0; i < 5; i++)
        {
            float z = -2250;
            for (int j = 0; j < 4; j++)
            {
                Entity381 ent0 = EntityMgr.inst.CreateEntity(entityType6, new Vector3(x, 0, z), Vector3.zero);
                ent0.desiredHeading = 90f;
                ent0.heading = 90f;
                ent0.maxSpeed = ent1.maxSpeed * 0.5f;
                ent0.gameObject.AddComponent<TestCaseEntity>();
                ent0.GetComponent<TestCaseEntity>().testCase = TestCase.Eight;
                ent0.GetComponent<TestCaseEntity>().move = new Vector3(10000, 0, z);
                ent0.GetComponent<TestCaseEntity>().added = false;
                z -= 200;
            }
            x -= 500;

        }

        DistanceMgr.inst.Initialize();

    }

    // Update is called once per frame
    void Update()
    {
    }
        
}

