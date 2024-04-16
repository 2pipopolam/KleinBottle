using System.Collections.Generic;
using UnityEngine;

public class KleinBottleRenderer : MonoBehaviour
{
    public Material material;
    
    void Start()
    {
        List<Vector3> vertexData = CreateVertices();
        Vector4 lightData = new Vector4(1.0f, 1.0f, 0.0f, 30.0f);
        Vector2 uvMode = new Vector2(0.0f, 0.0f); 
        
        CreateMesh(vertexData, lightData, uvMode);
    }

    List<Vector3> CreateVertices()
    {
        List<Vector3> vertices = new List<Vector3>();
        int nu = 70; // points u
        int nv = 30; // points v
        
        for (int i = 0; i < nu; i++)
        {
            float u = 0.0f + (2.0f * Mathf.PI * i) / (nu - 1);
            for (int j = 0; j < nv; j++)
            {
                float v = 0.0f + (2.0f * Mathf.PI * j) / (nv - 1);

                // Evaluate Klein bottle parametric equations
                float x = 2.0f / 15.0f * (3.0f + 5.0f * Mathf.Cos(u) * Mathf.Sin(u)) * Mathf.Sin(v);
                float y = -1.0f / 15.0f * Mathf.Sin(u) * (3.0f * Mathf.Cos(v) - 3.0f * Mathf.Pow(Mathf.Cos(u), 2) * Mathf.Cos(v) -
                                                          48.0f * Mathf.Pow(Mathf.Cos(u), 4) * Mathf.Cos(v) + 48.0f * Mathf.Pow(Mathf.Cos(u), 6) * Mathf.Cos(v) -
                                                          60.0f * Mathf.Sin(u) + 5.0f * Mathf.Cos(u) * Mathf.Cos(v) * Mathf.Sin(u) -
                                                          5.0f * Mathf.Pow(Mathf.Cos(u), 3) * Mathf.Cos(v) * Mathf.Sin(u) -
                                                          80.0f * Mathf.Pow(Mathf.Cos(u), 5) * Mathf.Cos(v) * Mathf.Sin(u) +
                                                          80.0f * Mathf.Pow(Mathf.Cos(u), 7) * Mathf.Cos(v) * Mathf.Sin(u));
                float z = -2.0f / 15.0f * Mathf.Cos(u) * (3.0f * Mathf.Cos(v) - 30.0f * Mathf.Sin(u) +
                                                          90.0f * Mathf.Pow(Mathf.Cos(u), 4) * Mathf.Sin(u) - 60.0f * Mathf.Pow(Mathf.Cos(u), 6) * Mathf.Sin(u) +
                                                          5.0f * Mathf.Cos(u) * Mathf.Cos(v) * Mathf.Sin(u));
                // Add vertex to the list of vectors3
                vertices.Add(new Vector3(x, y, z));
            }
        }
        return vertices;
    }

    void CreateMesh(List<Vector3> vertices, Vector4 lightData, Vector2 uvMode)
    {
        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        // Generate triangles
        List<int> triangles = new List<int>();
        int nu = 70; // points u
        int nv = 30; // points v
        for (int i = 0; i < nu - 1; i++)
        {
            for (int j = 0; j < nv - 1; j++)
            {
                int currentIndex = j + i * nv;
                int nextIndex = j + (i + 1) * nv;

                // Triangle 1
                triangles.Add(currentIndex);
                triangles.Add(currentIndex + 1);
                triangles.Add(nextIndex);

                // Triangle 2
                triangles.Add(currentIndex + 1);
                triangles.Add(nextIndex + 1);
                triangles.Add(nextIndex);
            }
        }
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        GameObject meshObject = new GameObject("KleinBottleMesh");
        meshObject.AddComponent<MeshFilter>().mesh = mesh;
        meshObject.AddComponent<MeshRenderer>().material = material;
    }
}