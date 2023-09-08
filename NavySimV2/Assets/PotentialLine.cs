using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotentialLine : MonoBehaviour
{
    public GameObject field;
    public GameObject middleField;
    public int numFields;
    public int length;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(middleField.transform.position, Vector3.up, 20*Time.deltaTime);
    }

    public void AddFields()
    {
        for (int i = 0; i < numFields; i++)
        {
            Vector3 position = gameObject.transform.position - (gameObject.transform.forward * length / 2) + (i / (1.0f * numFields) * gameObject.transform.forward * length);
            GameObject newField = Instantiate(field, transform);
            newField.transform.position = position;
        }

    }
}
