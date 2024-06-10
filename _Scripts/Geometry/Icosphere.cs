using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.Geometry
{
    // [RequireComponent (typeof(MeshFilter), typeof(MeshRenderer)) ]
    public class Icosphere : MonoBehaviour
    {
        
    // ================== References ==================
        [SerializeField] private Debugger debugger;
        [SerializeField] private GameObject grabbableVertexPrefab;
        [SerializeField] private Transform grabbableVertexContainer;

        // [SerializeField] private PlanetScriptableObject planet;


    // ================== From PlanetGenerator.cs ==================

        public float Radius = 0.25f;
        public Material PlanetMaterial;
        public int IcosphereSubdivisions = 1;
        public bool SmoothNormals = false;

        private List<MeshTriangle> MeshTriangles = new List<MeshTriangle>();
        private List<Vector3> Vertices = new List<Vector3>();

        private GameObject planetGameObject;
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        private Mesh planetMesh;


    // ================== Functionality ==================
        private static Vector3 origin = new Vector3(0, 0, 0);

        void Start() {
            // debugger.Log("Starting generation...");
            StartGeneration();
        }

        public void StartGeneration()
        {
            planetGameObject = this.gameObject;
            planetGameObject.transform.parent = transform;

            if(meshRenderer == null)
            {
                meshRenderer = planetGameObject.AddComponent<MeshRenderer>();
                meshRenderer.material = PlanetMaterial;
            }

            if(meshFilter == null)
            {
                meshFilter = planetGameObject.AddComponent<MeshFilter>();
            }
            
            planetMesh = new Mesh();
            planetMesh = new Mesh();

            debugger.Log("Making icosphere...");
            MakeIcosphere();

            debugger.Log("Icosphere made, calculating neighbors...");
            CalculateNeighbors();

            debugger.Log("Neighbors calculated, generating mesh...");
            GenerateMesh();
        }

    // ================== Initialize ==================

        public void MakeIcosphere()
        {
            // this.transform.localScale = Vector3.one * Radius;
            MeshTriangles = new List<MeshTriangle>();
            Vertices = new List<Vector3>();

            float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

            Vertices.Add(new Vector3(-1,  t,  0).normalized * Radius);
            Vertices.Add(new Vector3( 1,  t,  0).normalized * Radius);
            Vertices.Add(new Vector3(-1, -t,  0).normalized * Radius);
            Vertices.Add(new Vector3( 1, -t,  0).normalized * Radius);
            Vertices.Add(new Vector3( 0, -1,  t).normalized * Radius);
            Vertices.Add(new Vector3( 0,  1,  t).normalized * Radius);
            Vertices.Add(new Vector3( 0, -1, -t).normalized * Radius);
            Vertices.Add(new Vector3( 0,  1, -t).normalized * Radius);
            Vertices.Add(new Vector3( t,  0, -1).normalized * Radius);
            Vertices.Add(new Vector3( t,  0,  1).normalized * Radius);
            Vertices.Add(new Vector3(-t,  0, -1).normalized * Radius);
            Vertices.Add(new Vector3(-t,  0,  1).normalized * Radius);

            MeshTriangles.Add(new MeshTriangle( 0, 11,  5));
            MeshTriangles.Add(new MeshTriangle( 0,  5,  1));
            MeshTriangles.Add(new MeshTriangle( 0,  1,  7));
            MeshTriangles.Add(new MeshTriangle( 0,  7, 10));
            MeshTriangles.Add(new MeshTriangle( 0, 10, 11));
            MeshTriangles.Add(new MeshTriangle( 1,  5,  9));
            MeshTriangles.Add(new MeshTriangle( 5, 11,  4));
            MeshTriangles.Add(new MeshTriangle(11, 10,  2));
            MeshTriangles.Add(new MeshTriangle(10,  7,  6));
            MeshTriangles.Add(new MeshTriangle( 7,  1,  8));
            MeshTriangles.Add(new MeshTriangle( 3,  9,  4));
            MeshTriangles.Add(new MeshTriangle( 3,  4,  2));
            MeshTriangles.Add(new MeshTriangle( 3,  2,  6));
            MeshTriangles.Add(new MeshTriangle( 3,  6,  8));
            MeshTriangles.Add(new MeshTriangle( 3,  8,  9));
            MeshTriangles.Add(new MeshTriangle( 4,  9,  5));
            MeshTriangles.Add(new MeshTriangle( 2,  4, 11));
            MeshTriangles.Add(new MeshTriangle( 6,  2, 10));
            MeshTriangles.Add(new MeshTriangle( 8,  6,  7));
            MeshTriangles.Add(new MeshTriangle( 9,  8,  1));

            Subdivide(IcosphereSubdivisions);
        }

        public void Subdivide(int subdivisions)
        {
            var midPointCache = new Dictionary<int, int>();

            for (int i = 0; i < subdivisions; i++)
            {
                var newPolys = new List<MeshTriangle>();
                foreach (var poly in MeshTriangles)
                {
                    int a = poly.VertexIndices[0];
                    int b = poly.VertexIndices[1];
                    int c = poly.VertexIndices[2];

                    int ab = GetMidPointIndex(midPointCache, a, b);
                    int bc = GetMidPointIndex(midPointCache, b, c);
                    int ca = GetMidPointIndex(midPointCache, c, a);

                    newPolys.Add(new MeshTriangle(a, ab, ca));
                    newPolys.Add(new MeshTriangle(b, bc, ab));
                    newPolys.Add(new MeshTriangle(c, ca, bc));
                    newPolys.Add(new MeshTriangle(ab, bc, ca));
                }
                
                MeshTriangles = newPolys;
            }
        }

        public int GetMidPointIndex(Dictionary<int, int> cache, int indexA, int indexB)
        {
            int smallerIndex = Mathf.Min(indexA, indexB);
            int greaterIndex = Mathf.Max(indexA, indexB);
            int key = (smallerIndex << 16) + greaterIndex;

            // If a midpoint is already defined, just return it.

            int ret;
            if (cache.TryGetValue(key, out ret))
                return ret;

            // If we're here, it's because a midpoint for these two
            // vertices hasn't been created yet. Let's do that now!

            Vector3 p1 = Vertices[indexA];
            Vector3 p2 = Vertices[indexB];
            Vector3 middle = Vector3.Lerp(p1, p2, 0.5f).normalized * Radius;

            ret = Vertices.Count;
            Vertices.Add(middle);

            // Add our new midpoint to the cache so we don't have
            // to do this again. =)

            cache.Add(key, ret);
            return ret;
        }

        public void CalculateNeighbors()
        {
            foreach (MeshTriangle poly in MeshTriangles)
            {
                foreach (MeshTriangle other_poly in MeshTriangles)
                {
                    if (poly == other_poly)
                        continue;

                    if (poly.IsNeighbouring(other_poly))
                        poly.Neighbours.Add(other_poly);
                }
            }
        }

        public void GenerateMesh()
        {
            int vertexCount = MeshTriangles.Count * 3;

            int[] indices = new int[vertexCount];

            Vector3[] vertices = new Vector3[vertexCount];
            Vector3[] normals  = new Vector3[vertexCount];
            Color32[] colors   = new Color32[vertexCount];
            Vector2[] uvs      = new Vector2[vertexCount];

            for (int i = 0; i < MeshTriangles.Count; i++)
            {
                var poly = MeshTriangles[i];

                indices[i * 3 + 0] = i * 3 + 0;
                indices[i * 3 + 1] = i * 3 + 1;
                indices[i * 3 + 2] = i * 3 + 2;

                vertices[i * 3 + 0] = Vertices[poly.VertexIndices[0]];
                vertices[i * 3 + 1] = Vertices[poly.VertexIndices[1]];
                vertices[i * 3 + 2] = Vertices[poly.VertexIndices[2]];

                uvs[i * 3 + 0] = poly.UVs[0];
                uvs[i * 3 + 1] = poly.UVs[1];
                uvs[i * 3 + 2] = poly.UVs[2];

                colors[i * 3 + 0] = poly.Color;
                colors[i * 3 + 1] = poly.Color;
                colors[i * 3 + 2] = poly.Color;

                if(SmoothNormals)
                {
                    normals[i * 3 + 0] = Vertices[poly.VertexIndices[0]].normalized;
                    normals[i * 3 + 1] = Vertices[poly.VertexIndices[1]].normalized;
                    normals[i * 3 + 2] = Vertices[poly.VertexIndices[2]].normalized;
                }
                else
                {
                    Vector3 ab = Vertices[poly.VertexIndices[1]] - Vertices[poly.VertexIndices[0]];
                    Vector3 ac = Vertices[poly.VertexIndices[2]] - Vertices[poly.VertexIndices[0]];

                    Vector3 normal = Vector3.Cross(ab, ac).normalized;

                    normals[i * 3 + 0] = normal;
                    normals[i * 3 + 1] = normal;
                    normals[i * 3 + 2] = normal;
                }
            }

            planetMesh.vertices = vertices;
            planetMesh.normals  = normals;
            planetMesh.colors32 = colors;
            planetMesh.uv       = uvs;

            planetMesh.SetTriangles(indices, 0);

            meshFilter.mesh = planetMesh;

        }


    // ================== Nearest Vertex ==================
    
        public Vector3 NearestVertexTo(Vector3 point) {
            float minDistanceSqr = Mathf.Infinity;
            Vector3 nearestVertex = Vector3.zero;

            // convert point to local space
            point = transform.InverseTransformPoint(point);

            // scan all vertices to find nearest
            foreach (Vector3 vertex in planetMesh.vertices) {
                Vector3 diff = point - vertex;
                float distSqr = diff.sqrMagnitude;

                if (distSqr < minDistanceSqr) {
                    minDistanceSqr = distSqr;
                    nearestVertex = vertex;
                }
            }

            // convert nearest vertex back to world space
            return transform.TransformPoint(nearestVertex);
        }


    // ================== Interactions ==================

        public GameObject AddGrabbableVertex(Vector3 vertex) {
            // convert point to local space
            vertex = transform.InverseTransformPoint(vertex);

            // add new grabbable point
            GameObject grabbableVertex = Instantiate(grabbableVertexPrefab, grabbableVertexContainer);
            grabbableVertex.transform.Translate(vertex);

            // TODO: change GrabbableVertex to scriptable object
            // grabbableVertex.GetComponent<GrabbableVertex>().ConstraintVector = NormalFromOrigin(vertex);

            return grabbableVertex;
        }

        public void AddAllGrabbableVertices() {
            RemoveGrabbableVertices();

            foreach (Vector3 vertex in planetMesh.vertices) {
                AddGrabbableVertex(vertex);
            }
        }

        public void RemoveGrabbableVertices() {
            foreach (Transform child in grabbableVertexContainer) {
                Destroy(child.gameObject);
            }
        }

    // ================== "Normal" Vector ==================

        /// Returns a "normal" vector, from the origin to a given vertex.
        /// This is used to constrain the movement of a vertex.
        public Vector3 NormalFromOrigin(Vector3 vertex) {
            return (vertex - origin).normalized;
        }





    }







    // ================== 4 ==================
    static class CubeMeshData
    {
        public static Vector3[] vertices = {
            //north-side
            new Vector3( 1,  1,  1),
            new Vector3(-1,  1,  1),
            new Vector3(-1, -1,  1),
            new Vector3( 1, -1,  1),
            //south-side
            new Vector3(-1,  1, -1),
            new Vector3( 1,  1, -1),
            new Vector3( 1, -1, -1),
            new Vector3(-1, -1, -1),
        };

        public static int[][] faceTriangles = {
            //north-side
            new int[] {0, 1, 2, 3},
            //east-side
            new int[] {5, 0, 3, 6},
            //south-side
            new int[] {4, 5, 6, 7},
            //west-side
            new int[] {1, 4, 7, 2},
            //top-side
            new int[] {5, 4, 1, 0},
            //bottom-side
            new int[] {3, 2, 7, 6},
        };

        public static Vector3[] faceVertices(int dir, float scale) {
            Vector3[] fv = new Vector3[4];
            for (int i = 0; i < fv.Length; i++) {
                Vector3 vertex = vertices[faceTriangles[dir][i]];
                fv[i] = Vector3.Scale(vertex, new Vector3(scale, scale, scale));
            }
            return fv;
        }

    }
}