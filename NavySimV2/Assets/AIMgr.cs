﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMgr : MonoBehaviour
{
    public static AIMgr inst;
    private void Awake()
    {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << 9;// LayerMask.GetMask("Water");
    }

    public bool isPotentialFieldsMovement = false;
    public float potentialDistanceThreshold = 1000;
    public float attractionCoefficient = 500;
    public float attractiveExponent = -1;
    public float repulsiveCoefficient = 60000;
    public float repulsiveExponent = -2.0f;


    public RaycastHit hit;
    public int layerMask;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue, layerMask)) {
                //Debug.DrawLine(Camera.main.transform.position, hit.point, Color.yellow, 2); //for debugging
                Vector3 pos = hit.point;
                pos.y = 0;
                Entity381 ent = FindClosestEntInRadius(pos, rClickRadiusSq);
                if (ent == null) {
                    HandleMove(SelectionMgr.inst.selectedEntities, pos);
                } else {
                    if (Input.GetKey(KeyCode.LeftControl))
                        HandleIntercept(SelectionMgr.inst.selectedEntities, ent);
                    else
                        HandleFollow(SelectionMgr.inst.selectedEntities, ent);
                }
            } else {
                //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 1000, Color.white, 2);
            }
        }
    }

    //normal move
    public void HandleMove(List<Entity381> entities, Vector3 point)
    {
        foreach (Entity381 entity in entities) {
            Move m = new Move(entity, hit.point);
            UnitAI uai = entity.GetComponent<UnitAI>();
            AddOrSet(m, uai);
        }
    }

    //move for test cases
    public void HandleMove(Entity381 entity, Vector3 point)
    {
        Move m = new Move(entity, point);
        UnitAI uai = entity.GetComponent<UnitAI>();
        uai.AddCommand(m);
    }

    void AddOrSet(Command c, UnitAI uai)
    {
        if (Input.GetKey(KeyCode.LeftShift))
            uai.AddCommand(c);
        else
            uai.SetCommand(c);
    }



    public void HandleFollow(List<Entity381> entities, Entity381 ent)
    {
        foreach (Entity381 entity in SelectionMgr.inst.selectedEntities) {
            Follow f = new Follow(entity, ent, new Vector3(100, 0, 0));
            UnitAI uai = entity.GetComponent<UnitAI>();
            AddOrSet(f, uai);
        }
    }

    public void HandleFollow(Entity381 entity, Entity381 ent, Vector3 offset)
    {
        Follow f = new Follow(entity, ent, offset);
        UnitAI uai = entity.GetComponent<UnitAI>();
        AddOrSet(f, uai);
    }

    void HandleIntercept(List<Entity381> entities, Entity381 ent)
    {
        foreach (Entity381 entity in SelectionMgr.inst.selectedEntities) {
            Intercept intercept = new Intercept(entity, ent);
            UnitAI uai = entity.GetComponent<UnitAI>();
            AddOrSet(intercept, uai);
        }

    }

    public float rClickRadiusSq = 10000;
    public Entity381 FindClosestEntInRadius(Vector3 point, float rsq)
    {
        Entity381 minEnt = null;
        float min = float.MaxValue;
        foreach (Entity381 ent in EntityMgr.inst.entities) {
            float distanceSq = (ent.transform.position - point).sqrMagnitude;
            if (distanceSq < rsq) {
                if (distanceSq < min) {
                    minEnt = ent;
                    min = distanceSq;
                }
            }    
        }
        return minEnt;
    }
}
