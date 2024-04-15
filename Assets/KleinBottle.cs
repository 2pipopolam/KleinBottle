using System.Collections.Generic;
using UnityEngine;

public class KleinBottleRenderer : MonoBehaviour
{
    public Material material;
   
    void Start()
    {
        List<Vector3> vertexData = new List<Vector3>();
        List<int> triangles = new List<int>();
        CreateKleinBottle(vertexData, triangles);
    }

    void CreateKleinBottle(List<Vector3> vertices, List<int> triangles)
    {
        // The number of vertices = (NU * NV) * 2 (because half of the bottle is drawn 2 times)
        // the other half is mirrored and rotated 180 degrees along the Y axis
        
        int nu = 70; // Number of vertices of u
        int nv = 30; // Number of vertices of v

        // Generate vertices based on Klein bottle parametric equations
        for (int i = 0; i < nu; i++)
        {
            float u = 0.0f + (2.0f * Mathf.PI * i) / (nu - 1);
            for (int j = 0; j < nv; j++)
            {
                float v = 0.0f + Mathf.PI * j / (nv - 1);

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

                // Add vertex to the list
                vertices.Add(new Vector3(x, y, z));
            }
        }

        // Generate triangles
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

        // Create mirrored version of the Klein bottle
        List<Vector3> mirroredVertices = new List<Vector3>(vertices.Count);
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            mirroredVertices.Add(new Vector3(vertex.x, vertex.y, -vertex.z)); // Mirror along the z-axis
        }

        // Add the mirrored vertices to the existing list
        int originalVertexCount = vertices.Count;
        vertices.AddRange(mirroredVertices);

        List<int> copiedTriangles = new List<int>(triangles);

        foreach (int index in copiedTriangles) triangles.Add(index + originalVertexCount); // Add mirrored indices
        
        // Rotation axis (Y)
        Vector3 rotationAxis = Vector3.up;

        // Rotate 180 deg
        for (int i = originalVertexCount; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            vertices[i] = Quaternion.AngleAxis(180f, rotationAxis) * vertex;
        }

        // Combine vertices and triangles
        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

        // Create GameObject and render
        GameObject meshObject = new GameObject("KleinBottleMesh");
        meshObject.AddComponent<MeshFilter>().mesh = mesh;
        meshObject.AddComponent<MeshRenderer>().material = material;
    }
}