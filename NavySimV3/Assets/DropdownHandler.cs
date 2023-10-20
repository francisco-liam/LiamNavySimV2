using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class DropdownHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public int TestCase;
    public int ShipNum;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public EntityType HandleDropDown(int val)
    {
        if (val == 0)
            return EntityType.DDG51;
        else if (val == 1)
            return EntityType.TugBoat;
        else if (val == 2)
            return EntityType.Tanker;
        else if (val == 3)
            return EntityType.Container;
        else if (val == 4)
            return EntityType.MineSweeper;
        else if (val == 5)
            return EntityType.OilServiceVessel;
        else if (val == 6)
            return EntityType.OrientExplorer;
        else if (val == 7)
            return EntityType.PilotVessel;
        else
            return EntityType.SmitHouston;

    }

    public TestCase HandleTestCase(int val)
    {
        if (val == 0)
            return global::TestCase.One;
        else if (val == 1)
            return global::TestCase.Two;
        else if (val == 2)
            return global::TestCase.Three;
        else if (val == 3)
            return global::TestCase.Four;
        else if (val == 4)
            return global::TestCase.Five;
        else if (val == 5)
            return global::TestCase.Six;
        else if (val == 6)
            return global::TestCase.Seven;
        else if (val == 7)
            return global::TestCase.Eight;
        else if (val == 8)
            return global::TestCase.Nine;
        else if (val == 9)
            return global::TestCase.Ten;
        else
            return global::TestCase.Test;
    }

    public void ChangeShip(int val)
    {
        if (TestCase == 1)
        {
            if (ShipNum == 0)
                TestCases.inst.entityType6 = HandleDropDown(val);
            if (ShipNum == 1)
                TestCases.inst.entityType7 = HandleDropDown(val);
        }
        else
        {
            if (ShipNum == 3)
                TestCases.inst.entityType3 = HandleDropDown(val);
            if (ShipNum == 4)
                TestCases.inst.entityType4 = HandleDropDown(val);
            if (ShipNum == 5)
                TestCases.inst.entityType5 = HandleDropDown(val);
            if (ShipNum == 6)
                TestCases.inst.testCase = HandleTestCase(val);
        }
    }

    public void ChangePotCalc(int val)
    {
        if (val == 0)
            DistanceMgr.inst.fraction = 1;
        if (val == 1)
            DistanceMgr.inst.fraction = 2;
        if (val == 2)
            DistanceMgr.inst.fraction = 5;
        if (val == 3)
            DistanceMgr.inst.fraction = 10;
    }
}
