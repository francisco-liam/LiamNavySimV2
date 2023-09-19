using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestCaseEntity : MonoBehaviour
{
    public TestCase testCase;
    public Entity381 ownEnt;
    public UnitAI ownAI;
    public bool running;

    [Header("Test Case #2-4 Variables")]
    public Entity381 ship1;
    public Entity381 ship2;
    public Entity381 ship3;
    public Vector3 offset;
    public int commandNumber;

    [Header("Test Case #5 Variables")]
    public Vector3 move; 
    public bool added;

    [Header("Stats")]
    public float collisionsTime;
    public float travelTime;
    public float travelDistance;

    
    
    // Start is called before the first frame update
    void Start()
    {
        commandNumber = 0;
        offset = Vector3.zero;
        ownEnt = gameObject.GetComponent<Entity381>();
        ownAI = gameObject.GetComponent<UnitAI>();
        running = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (testCase == TestCase.One)
        {
            if (!added)
            {
                AIMgr.inst.HandleMove(ownEnt, new Vector3(-1400, 0, -1500));
                AIMgr.inst.HandleMove(ownEnt, new Vector3(-1200, 0, -1500));
                AIMgr.inst.HandleMove(ownEnt, new Vector3(-1000, 0, -1500));
                added = true;
            }
        }
        if (testCase == TestCase.Two || testCase == TestCase.Three || testCase == TestCase.Four)
            TestCase2to4();
        else if (!added)
        {
            AIMgr.inst.HandleMove(ownEnt, move);
            added = true;
        }

        if (running)
        {
            travelTime += Time.deltaTime;
            travelDistance += Time.deltaTime * ownEnt.speed;
        }

        if(testCase != TestCase.Two && testCase != TestCase.Three && testCase != TestCase.Four)
        {
            if(ownAI.commands.Count == 0)
                running = false;
        }

    }

    void TestCase2to4()
    {
        setOffset();
        if (commandNumber == 0 && ownAI.commands.Count == 0)
        {
            AIMgr.inst.HandleFollow(ownEnt, ship1, offset);
            commandNumber++;
        }
        if (commandNumber == 1 && ownAI.commands.Count == 0)
        {
            AIMgr.inst.HandleFollow(ownEnt, ship2, offset);
            commandNumber++;
        }
        if (commandNumber == 2 && ownAI.commands.Count == 0)
        {
            AIMgr.inst.HandleFollow(ownEnt, ship3, offset);
            commandNumber++;
        }
        if (commandNumber == 3 && ownAI.commands.Count == 0)
        {
            AIMgr.inst.HandleFollow(ownEnt, ship3, offset);
            commandNumber++;
        }

        if (ownAI.commands.Count > 0)
        {
            if ((ownAI.commands[0].movePosition - ownEnt.position).sqrMagnitude < 15000 && commandNumber != 4)
            {
                ownAI.commands[0].Stop();
                ownAI.commands.RemoveAt(0);
            }
            else if((ownAI.commands[0].movePosition - ownEnt.position).sqrMagnitude < 15000 && commandNumber == 4)
            {
                running = false;
            }
        }

        offset = Vector3.zero;
    }

    void setOffset()
    {
        if (commandNumber == 0)
        {
            setZOffset(ship1);
            offset += new Vector3(100, 0, 0);
        }
        if (commandNumber == 1)
        {
            setZOffset(ship2);
            offset += new Vector3(100, 0, 0);
        }
        if (commandNumber == 2)
        {
            setZOffset(ship3);
            offset += new Vector3(100, 0, 0);
        }
        if (commandNumber == 3)
        {
            setZOffset(ship3);
            offset += new Vector3(-100, 0, 0);
        }
            
    }

    void setZOffset(Entity381 ship)
    {
        if (testCase == TestCase.Two)
            offset.z = (ship.length/2);
        if (testCase == TestCase.Three)
            offset.z = -(ship.length/2);
        if (testCase == TestCase.Four)
            offset.z = 0;
    }

    void OnTriggerStay(Collider other)
    {
        if (running && other.gameObject.transform.tag == "Ship")
        {
            collisionsTime += Time.deltaTime;
        }
    }
}
