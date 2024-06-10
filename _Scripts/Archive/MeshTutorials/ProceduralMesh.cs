using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ProceduralMesh : MonoBehaviour {

    Mesh mesh;

    Vector3[] vertices;
    // Vector2[] uv;
    int[] triangles;
    
    void Awake() {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void Start() {
        MakeMeshData();
        UpdateMesh();
    }

    void MakeMeshData() {
        vertices = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
        };
        // uv = new Vector2[] {
        //     new Vector2(0, 0),
        //     new Vector2(0, 1),
        //     new Vector2(1, 1),
        //     new Vector2(1, 0)
        // };
        triangles = new int[] { 0, 1, 2, 2, 1, 3 };
    }

    void UpdateMesh() {
        mesh.Clear();
        mesh.vertices = vertices;
        // mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    public Vector3 NearestVertexTo(Vector3 point) {
        // convert point to local space
        point = transform.InverseTransformPoint(point);

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        float minDistanceSqr = Mathf.Infinity;
        Vector3 nearestVertex = Vector3.zero;

        // scan all vertices to find nearest
        foreach (Vector3 vertex in mesh.vertices) {
            Vector3 diff = point-vertex;
            float distSqr = diff.sqrMagnitude;

            if (distSqr < minDistanceSqr) {
                minDistanceSqr = distSqr;
                nearestVertex = vertex;
            }
        }

        // convert nearest vertex back to world space
        return transform.TransformPoint(nearestVertex);

    }
}
