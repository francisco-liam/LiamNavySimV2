using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShipData
{
    public float mass;
    public float length;
    public float repulsiveCoefficient;
    public float repulsiveExpoenent;
    public float targetAngleCoefficient;
    public float targetAngleExponent;
    public float relativeBearingCoefficient;
    public float relativeBearingExponent;
    public int numFields;

    public Vector3 position;

};

public class GraphComputeShader : MonoBehaviour
{
    public ComputeShader computeShader;
    public RenderTexture renderTexture;
    public List<ShipData> allShipData;
    int totalStructSize;
    
    // Start is called before the first frame update
    void Start()
    {
        renderTexture = new RenderTexture(256, 256, 24); 
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();

        computeShader.SetTexture(0, "Result", renderTexture);
        computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);
        allShipData = new List<ShipData>();

        totalStructSize = sizeof(float) * 11 + sizeof(int);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateData()
    {
        allShipData.Clear();
        foreach (Entity381 entity in EntityMgr.inst.entities)
        {
            ShipData shipData = new ShipData();
            shipData.mass = entity.mass;
            shipData.length = entity.length;
            shipData.repulsiveCoefficient = entity.repulsiveCoefficient;
            shipData.repulsiveExpoenent = entity.repulsiveExponent;
            shipData.targetAngleCoefficient = entity.taCoefficient;
            shipData.targetAngleExponent = entity.taExponent;
            shipData.relativeBearingCoefficient = entity.rbCoefficient;
            shipData.relativeBearingExponent = entity.rbExponent;
            shipData.numFields = entity.numFields;
            shipData.position = entity.position;

            allShipData.Add(shipData);
        }
    }

    public void SetComputeBuffer()
    {
        
        ComputeBuffer dataBuffer = new ComputeBuffer(allShipData.Count, totalStructSize);
        dataBuffer.SetData(allShipData);

        computeShader.SetBuffer(0, "ships", dataBuffer);
        computeShader.Dispatch(0, allShipData.Count / 10, 1, 1);

        dataBuffer.GetData(allShipData.ToArray());

        dataBuffer.Dispose();
    }
}
