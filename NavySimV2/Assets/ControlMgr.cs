using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMgr : MonoBehaviour
{
    public static ControlMgr inst;
    public GameObject test;
    public GameObject test2;
    private void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float deltaSpeed = 1;
    public float deltaHeading = 2;
    public float timeFactor = 10f;

    // Update is called once per frame
    void Update()
    {
        if (SelectionMgr.inst.selectedEntity != null) {
            if (Input.GetKeyUp(KeyCode.UpArrow))
                SelectionMgr.inst.selectedEntity.desiredSpeed += deltaSpeed;
            if (Input.GetKeyUp(KeyCode.DownArrow))
                SelectionMgr.inst.selectedEntity.desiredSpeed -= deltaSpeed;
            SelectionMgr.inst.selectedEntity.desiredSpeed =
                Utils.Clamp(SelectionMgr.inst.selectedEntity.desiredSpeed, SelectionMgr.inst.selectedEntity.minSpeed, SelectionMgr.inst.selectedEntity.maxSpeed);

            if (Input.GetKeyUp(KeyCode.LeftArrow))
                SelectionMgr.inst.selectedEntity.desiredHeading -= deltaHeading;
            if (Input.GetKeyUp(KeyCode.RightArrow))
                SelectionMgr.inst.selectedEntity.desiredHeading += deltaHeading;
            SelectionMgr.inst.selectedEntity.desiredHeading = Utils.Degrees360(SelectionMgr.inst.selectedEntity.desiredHeading);
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            if (Time.timeScale == 1.0f)
                Time.timeScale = timeFactor;
            else
                Time.timeScale = 1.0f;
        }

        if(Input.GetKeyUp(KeyCode.P))
        {
            TestCases.inst.ToggleFields(SelectionMgr.inst.selectedEntities);
        }

        if (Input.GetKey(KeyCode.G))
            TestCases.inst.MoveFields(SelectionMgr.inst.selectedEntities, "FrontFields", true);
        if (Input.GetKey(KeyCode.H))
            TestCases.inst.MoveFields(SelectionMgr.inst.selectedEntities, "FrontFields", false);
        if (Input.GetKey(KeyCode.B))
            TestCases.inst.MoveFields(SelectionMgr.inst.selectedEntities, "BackFields", true);
        if (Input.GetKey(KeyCode.N))
            TestCases.inst.MoveFields(SelectionMgr.inst.selectedEntities, "BackFields", false);

    }

    
    //------------------------------------------
}
