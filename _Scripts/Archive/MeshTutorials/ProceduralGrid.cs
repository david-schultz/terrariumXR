using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer)) ]
public class ProceduralGrid : MonoBehaviour {
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    // grid settings
    public float cellSize = 1;
    public Vector3 gridOffset;
    public int gridSizeX, gridSizeY;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void Start()
    {
        MakeDiscreteProceduralGrid();
        // MakeContiguousProceduralGrid();
        UpdateMesh();
    }

// ================== 1 ==================
    void MakeDiscreteProceduralGrid()
    {
        //set array sizes
        vertices = new Vector3[gridSizeX * gridSizeY * 4];
        triangles = new int[gridSizeX * gridSizeY * 6];

        //set tracker integers
        int v = 0;
        int t = 0;

        //set vertex offset
        //brings the "origin" of each quad to the center of the grid cell
        float vertexOffset = cellSize * 0.5f;

        //populate vertices and triangles arrays
        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                //calculate vertex positions
                Vector3 cellOffset = new Vector3(x * cellSize, 0, y * cellSize);

                vertices[v + 0] = new Vector3(-vertexOffset, 0, -vertexOffset) + cellOffset + gridOffset;
                vertices[v + 1] = new Vector3(-vertexOffset, 0,  vertexOffset) + cellOffset + gridOffset;
                vertices[v + 2] = new Vector3( vertexOffset, 0, -vertexOffset) + cellOffset + gridOffset;
                vertices[v + 3] = new Vector3( vertexOffset, 0,  vertexOffset) + cellOffset + gridOffset;

                //set vertex order for the quad
                triangles[t + 0] = v + 0;
                triangles[t + 1] = v + 1;
                triangles[t + 2] = v + 2;

                triangles[t + 3] = v + 2;
                triangles[t + 4] = v + 1;
                triangles[t + 5] = v + 3;

                //update trackers
                v += 4;
                t += 6;
            }
        }
    }

// ================== 2 ==================
    void MakeContiguousProceduralGrid()
    {
        //set array sizes
        vertices = new Vector3[(gridSizeX + 1) * (gridSizeY + 1)];
        triangles = new int[gridSizeX * gridSizeY * 6];

        //set tracker integers
        int v = 0;
        int t = 0;

        //set vertex offset
        //brings the "origin" of each quad to the center of the grid cell
        float vertexOffset = cellSize * 0.5f;

        //populate vertex grid
        for (int x = 0; x < gridSizeX + 1; x++) {
            for (int y = 0; y < gridSizeY + 1; y++) {
                vertices[v] = new Vector3((x * cellSize) - vertexOffset, 0, (y * cellSize) - vertexOffset);
                v++;
            }
        }

        //reset vertex tracker
        v = 0;

        //setting each cell's triangles
        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                triangles[t + 0] = v + 0;
                triangles[t + 1] = v + 1;
                triangles[t + 2] = v + (gridSizeX + 1);

                triangles[t + 3] = v + (gridSizeX + 1);
                triangles[t + 4] = v + 1;
                triangles[t + 5] = v + (gridSizeX + 1) + 1;

                //update trackers
                v++;
                t += 6;
            }
            //skip the last vertex in the row
            v++;
        }
    }

// ================== 3 ==================

    void UpdateMesh() {
        mesh.Clear();
        mesh.vertices = vertices;
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
