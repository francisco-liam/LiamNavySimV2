using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
//using UnityEngine.Experimental.PlayerLoop;
using Color = UnityEngine.Color;


public struct ShipData
{
    public float mass;
    public float length;
    public float attractionCoefficient;
    public float attractiveExponent;
    public float repulsiveCoefficient;
    public float repulsiveExponent;
    public float targetAngleCoefficient;
    public float targetAngleExponent;
    public float relativeBearingCoefficient;
    public float relativeBearingExponent;
    public int numFields;
    public Vector3 position;
    public float heading;
    public Vector3 fieldPos;
    public Vector3 movePosition;
    public Vector3 frontPos;
    public int commands;

};

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class GraphPlane : MonoBehaviour
{
    Mesh mesh;
    MeshFilter meshFilter;

    public List<Vector3> vertices;
    List<int> triangles;
    public List<ShipData> allShipData;
    public float[] test;

    public ComputeShader potentialShader;
    public Vector2 size;
    public int resolution;
    public bool entSpecific;
    public Entity381 entity;

    void Awake()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
    }

    // Start is called before the first frame update
    void Start()
    {
        size = GraphMgr.inst.size;
        resolution = GraphMgr.inst.resolution;
        resolution = Mathf.Clamp(resolution, 0, 1000);
        GeneratePlane(size, resolution);
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        allShipData = new List<ShipData>();
    }

    // Update is called once per frame
    void Update()
    {
        csUpdateHeights(entity);
        AssignMesh();
    }

    void SetSizeAndResolutiion()
    {
        size = GraphMgr.inst.size;
        resolution = GraphMgr.inst.resolution;
    }

    void GeneratePlane(Vector2 size, int resolution)
    {
        vertices = new List<Vector3>();
        float xPerStep = size.x / resolution;
        float yPerStep = size.y / resolution;
        for (int y = 0; y < resolution + 1; y++)
        {
            for (int x = 0; x < resolution + 1; x++)
            {
                vertices.Add(new Vector3(x * xPerStep, 0, y * yPerStep));
            }
        }

        triangles = new List<int>();

        int vert = 0;

        for (int row = 0; row < resolution; row++)
        {
            for (int col = 0; col < resolution; col++)
            {

                triangles.Add(vert + 0);
                triangles.Add(vert + resolution + 1);
                triangles.Add(vert+1);

                triangles.Add(vert +1 );
                triangles.Add(vert + resolution + 1);
                triangles.Add(vert + resolution + 2);
                vert++;
            }
        }
    }

    void AssignMesh()
    {
        mesh.vertices = vertices.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    void UpdateHeights()
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            Vector3 vertexPos = transform.TransformPoint(vertex);
            vertexPos.y = 0;
            if (!entSpecific)
                vertex.y = (CalculatePotential(vertexPos) / GraphMgr.inst.maxMag) * 400f;
            else
                vertex.y = (CalculatePotential(vertexPos, entity) / GraphMgr.inst.maxMag) * 400;
            vertices[i] = vertex;
        }
    }
    
    void csUpdateHeights(Entity381 currentEnt)
    {
        updateFieldData();
        ComputeBuffer shipsBuffer = new ComputeBuffer(allShipData.Count, sizeof(float)*23 + sizeof(int)*2);
        shipsBuffer.SetData(allShipData);

        int sizeOfVec3 = sizeof(float) * 3;

        ComputeBuffer vertexBuffer = new ComputeBuffer(vertices.Count, sizeOfVec3);
        Vector3[] worldVert = vertices.ToArray();
        transform.TransformPoints(worldVert);
        vertexBuffer.SetData(worldVert);

        potentialShader.SetInt("numShips", EntityMgr.inst.entities.Count);
        potentialShader.SetInt("entity", EntityMgr.inst.entities.IndexOf(currentEnt));
        potentialShader.SetBuffer(0, "ships", shipsBuffer);
        potentialShader.SetBuffer(0, "positions", vertexBuffer);

        potentialShader.Dispatch(0, vertices.Count / 64, 1, 1);

        vertexBuffer.GetData(worldVert);
        transform.InverseTransformPoints(worldVert);
        vertices = new List<Vector3>(worldVert);

        shipsBuffer.Dispose();
        vertexBuffer.Dispose();
    }

    float CalculatePotential(Vector3 position)
    {

        Vector3 repulsivePotential;
        float magnitude;
        repulsivePotential = Vector3.zero;

        foreach (Entity381 ent in EntityMgr.inst.entities)
        {
            foreach (Vector3 fp in ent.fieldPos)
            {
                Vector3 diff = fp - position;
                float dist = diff.magnitude;
                Vector3 direc = diff.normalized;

                if (!ent.underway)
                {
                    if (dist < 1000)
                    {
                        repulsivePotential += direc * ent.mass / 40 * ent.length / 20 *
                           ent.repulsiveCoefficient / ent.numFields * Mathf.Pow(dist, ent.repulsiveExponent);
                    }
                }
                else
                {
                    if (dist < 1000)
                    {
                        if (fp != ent.front)
                        {
                            repulsivePotential += direc * ent.mass / 40 * ent.length / 20 *
                                ent.repulsiveCoefficient / ent.numFields * Mathf.Pow(dist, ent.repulsiveExponent);
                        }
                        else
                        {
                            repulsivePotential += direc * ent.mass / 40 * ent.length / 20 *
                                ent.repulsiveCoefficient / 2 * Mathf.Pow(dist, ent.repulsiveExponent);
                        }
                    }
                }
            }
        }

        magnitude = Utils.Clamp(repulsivePotential.magnitude, 0, GraphMgr.inst.maxMag);

        return magnitude;
    }

    float CalculatePotential(Vector3 position, Entity381 entity)
    {

        Vector3 repulsivePotential;
        Vector3 attractivePotential = Vector3.zero;
        float magnitude;
        float potentialMag;

        if (entity.transform.GetComponent<UnitAI>().commands.Count != 0)
        {
            attractivePotential = entity.transform.GetComponent<UnitAI>().commands[0].movePosition - position;
            Vector3 tmp = attractivePotential.normalized;
            attractivePotential = tmp *
                entity.attractionCoefficient * Mathf.Pow(attractivePotential.magnitude, entity.attractiveExponent);
        }

        Potential p;
        repulsivePotential = Vector3.zero;
        foreach (Entity381 ent in EntityMgr.inst.entities)
        {
            if (ent == entity) continue;
            p = DistanceMgr.inst.GetPotential(entity, ent);
            foreach (Vector3 fp in ent.fieldPos)
            {
                //float coeff = SituationCases(p);

                Vector3 diff = fp - position;
                float dist = diff.magnitude;
                Vector3 direc = diff.normalized;


                float taCoeff = Mathf.Sin((p.targetAngle) * Mathf.Deg2Rad);
                float rbCoeff = Mathf.Sin((p.relativeBearingDegrees - 90f) * Mathf.Deg2Rad);
                Vector3 taField = direc * p.target.taCoefficient * ent.mass / 40 * ent.length / 20 * taCoeff * Mathf.Pow(dist, p.target.taExponent);
                Vector3 rbField = direc * p.target.rbCoefficient * ent.mass / 40 * ent.length / 20 * rbCoeff * Mathf.Pow(dist, p.target.rbExponent);

                if (!ent.underway)
                {
                    if (dist < 1000)
                    {
                        repulsivePotential += direc * ent.mass / 40 * ent.length / 20 *
                                ent.repulsiveCoefficient / ent.numFields * Mathf.Pow(dist, ent.repulsiveExponent);
                        if (taCoeff > 0)
                        {
                            repulsivePotential += taField;
                        }
                        else
                        {
                            attractivePotential += taField;
                        }

                        if (rbCoeff  > 0)
                        {
                            repulsivePotential += rbField;
                        }
                        else
                        {
                            attractivePotential += rbField;
                        }
                    }
                }
                else
                {
                    if (dist < 1000)
                    {
                        if (fp != ent.front)
                        {
                            repulsivePotential += direc * ent.mass / 40 * ent.length / 20 *
                                ent.repulsiveCoefficient / ent.numFields * Mathf.Pow(dist, ent.repulsiveExponent);
                            if(taCoeff > 0)
                            {
                                repulsivePotential += taField;
                            }
                            else
                            {
                                attractivePotential += taField;
                            }

                            if(rbCoeff > 0)
                            {
                                repulsivePotential += rbField;
                            }
                            else
                            {
                                attractivePotential += rbField;
                            }
                        }
                        else
                        {
                            repulsivePotential += direc * ent.mass / 40 * ent.length / 20 *
                                ent.repulsiveCoefficient / ent.numFields * Mathf.Pow(dist, ent.repulsiveExponent);
                            if (taCoeff > 0)
                            {
                                repulsivePotential += taField;
                            }
                            else
                            {
                                attractivePotential += taField;
                            }

                            if (rbCoeff > 0)
                            {
                                repulsivePotential += rbField;
                            }
                            else
                            {
                                attractivePotential += rbField;
                            }
                        }
                    }
                }
            }

        }

        potentialMag = repulsivePotential.magnitude - attractivePotential.magnitude;

        magnitude = Utils.Clamp(potentialMag, -GraphMgr.inst.maxMag, GraphMgr.inst.maxMag);

        return magnitude;

    }

    float SituationCases(Potential p)
    {
        float coeff = 1;

        if (Utils.isBetween(90, 180, p.relativeBearingDegrees) && Utils.isBetween(270, 0, p.targetAngle))
            coeff = 0;
        if (Utils.isBetween(210, 310, p.relativeBearingDegrees) && Utils.isBetween(300, 40, p.targetAngle))
            coeff = 0;
        if (p.cpaInfo.time == 0)
            coeff = 0;

        return coeff;
    }

    //now handled by shader graph
    Color[] ChangeColors()
    {
        Color[] colors = new Color[vertices.Count];
        if (entSpecific)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                colors[i] = Color.Lerp(Color.green, Color.red, (vertices[i].y + 400f) / 800f);
            }
        }
        else
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                colors[i] = Color.Lerp(Color.green, Color.red, vertices[i].y / 400f);
            }
        }


        return colors;
    }

    public void updateFieldData()
    {
        allShipData = new List<ShipData>();
        foreach (Entity381 entity in EntityMgr.inst.entities)
        {
            ShipData shipData = new ShipData();
            shipData.mass = entity.mass;
            shipData.length = entity.length;
            shipData.attractionCoefficient = entity.attractionCoefficient;
            shipData.attractiveExponent = entity.attractiveExponent;
            shipData.repulsiveCoefficient = entity.repulsiveCoefficient;
            shipData.repulsiveExponent = entity.repulsiveExponent;
            shipData.targetAngleCoefficient = entity.taCoefficient;
            shipData.targetAngleExponent = entity.taExponent;
            shipData.relativeBearingCoefficient = entity.rbCoefficient;
            shipData.relativeBearingExponent = entity.rbExponent;
            shipData.numFields = entity.numFields;
            shipData.position = entity.position;
            shipData.heading = entity.heading;
            shipData.fieldPos = entity.fieldPos[0];
            shipData.frontPos = entity.front;
            shipData.commands = entity.transform.GetComponent<UnitAI>().commands.Count;
            if (entity.transform.GetComponent<UnitAI>().commands.Count != 0)
                shipData.movePosition = entity.transform.GetComponent<UnitAI>().commands[0].movePosition;
            else
                shipData.movePosition = Vector3.zero;

            allShipData.Add(shipData);
        }
    }
}