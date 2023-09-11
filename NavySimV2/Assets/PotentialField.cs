﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialField : MonoBehaviour
{
    public float potentialDistanceThreshold = 500;
    public float repulsiveCoefficient = 1000;
    public float repulsiveExponent = -2.0f;
    public Entity381 entity;

    // Start is called before the first frame update
    void Start()
    {
        potentialDistanceThreshold = entity.potentialDistanceThreshold;
        repulsiveCoefficient = entity.repulsiveCoefficient;
        repulsiveExponent = entity.repulsiveExponent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
