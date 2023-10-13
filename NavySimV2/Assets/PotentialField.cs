using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialField : MonoBehaviour
{
    public float repulsiveCoefficient = 1000;
    public float repulsiveExponent = -2.0f;
    public float attractiveCoefficient;
    public float attractiveExponent;
    public float potentialThreshold;


    public Entity381 entity;

    // Start is called before the first frame update
    void Start()
    {
        if(entity != null)
        {
            repulsiveCoefficient = entity.repulsiveCoefficient;
            repulsiveExponent = entity.repulsiveExponent;
            entity.fields.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
