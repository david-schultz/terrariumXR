using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Note: Using a north-based coordinate system.
//Reference: https://youtu.be/bnmr_At2R0s?si=1SoFZTUouIKO4uDJ

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer)) ]
public class ProceduralCube : MonoBehaviour {

    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;

    void Awake() {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void Start() {
        MakeCube();
        UpdateMesh();
    }

// ================== 1 ==================
    void MakeCube() {
        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int i = 0; i < 6; i++) {
            MakeFace(i);
        }
    }

    void MakeFace(int dir) {
        vertices.AddRange(CubeMeshData.faceVertices(dir, 0.1f));

        int vCount = vertices.Count;
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 4 + 1);
        triangles.Add(vCount - 4 + 2);
        triangles.Add(vCount - 4);
        triangles.Add(vCount - 4 + 2);
        triangles.Add(vCount - 4 + 3);
    }

    void UpdateMesh() {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
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
