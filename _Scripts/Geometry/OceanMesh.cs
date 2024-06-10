using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR.EventSystem;

namespace TerrariumXR.Geometry
{
    [RequireComponent (typeof(MeshFilter), typeof(MeshRenderer)) ]
    public class OceanMesh : MonoBehaviour
    {
        [SerializeField] Material _material;
        [SerializeField] float _radius;
        [SerializeField] int _subdivisions;
        [SerializeField] bool _shouldSmoothNormals;
        private List<Vector3> _vertices;
        private List<MeshTriangle> _meshTriangles;
        // Start is called before the first frame update
        void Start()
        {
            MakeIcosphere(_radius);
            CalculateNeighbors();
            Subdivide(_subdivisions, _radius);
            UpdateMesh();
        }

        private void MakeIcosphere(float radius)
        {
            _meshTriangles = new List<MeshTriangle>();
            _vertices = new List<Vector3>();

            float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

            _vertices.Add(new Vector3(-1,  t,  0).normalized * radius);
            _vertices.Add(new Vector3( 1,  t,  0).normalized * radius);
            _vertices.Add(new Vector3(-1, -t,  0).normalized * radius);
            _vertices.Add(new Vector3( 1, -t,  0).normalized * radius);
            _vertices.Add(new Vector3( 0, -1,  t).normalized * radius);
            _vertices.Add(new Vector3( 0,  1,  t).normalized * radius);
            _vertices.Add(new Vector3( 0, -1, -t).normalized * radius);
            _vertices.Add(new Vector3( 0,  1, -t).normalized * radius);
            _vertices.Add(new Vector3( t,  0, -1).normalized * radius);
            _vertices.Add(new Vector3( t,  0,  1).normalized * radius);
            _vertices.Add(new Vector3(-t,  0, -1).normalized * radius);
            _vertices.Add(new Vector3(-t,  0,  1).normalized * radius);

            _meshTriangles.Add(new MeshTriangle( 0, 11,  5));
            _meshTriangles.Add(new MeshTriangle( 0,  5,  1));
            _meshTriangles.Add(new MeshTriangle( 0,  1,  7));
            _meshTriangles.Add(new MeshTriangle( 0,  7, 10));
            _meshTriangles.Add(new MeshTriangle( 0, 10, 11));
            _meshTriangles.Add(new MeshTriangle( 1,  5,  9));
            _meshTriangles.Add(new MeshTriangle( 5, 11,  4));
            _meshTriangles.Add(new MeshTriangle(11, 10,  2));
            _meshTriangles.Add(new MeshTriangle(10,  7,  6));
            _meshTriangles.Add(new MeshTriangle( 7,  1,  8));
            _meshTriangles.Add(new MeshTriangle( 3,  9,  4));
            _meshTriangles.Add(new MeshTriangle( 3,  4,  2));
            _meshTriangles.Add(new MeshTriangle( 3,  2,  6));
            _meshTriangles.Add(new MeshTriangle( 3,  6,  8));
            _meshTriangles.Add(new MeshTriangle( 3,  8,  9));
            _meshTriangles.Add(new MeshTriangle( 4,  9,  5));
            _meshTriangles.Add(new MeshTriangle( 2,  4, 11));
            _meshTriangles.Add(new MeshTriangle( 6,  2, 10));
            _meshTriangles.Add(new MeshTriangle( 8,  6,  7));
            _meshTriangles.Add(new MeshTriangle( 9,  8,  1));
        }

        private void Subdivide(int subdivisions, float radius)
        {
            var midPointCache = new Dictionary<int, int>();

            for (int i = 0; i < subdivisions; i++)
            {
                var newPolys = new List<MeshTriangle>();
                foreach (var poly in _meshTriangles)
                {
                    int a = poly.VertexIndices[0];
                    int b = poly.VertexIndices[1];
                    int c = poly.VertexIndices[2];

                    int ab = GetMidPointIndex(midPointCache, a, b, radius);
                    int bc = GetMidPointIndex(midPointCache, b, c, radius);
                    int ca = GetMidPointIndex(midPointCache, c, a, radius);

                    newPolys.Add(new MeshTriangle(a, ab, ca));
                    newPolys.Add(new MeshTriangle(b, bc, ab));
                    newPolys.Add(new MeshTriangle(c, ca, bc));
                    newPolys.Add(new MeshTriangle(ab, bc, ca));
                }
                
                _meshTriangles = newPolys;
            }
        }

        private int GetMidPointIndex(Dictionary<int, int> cache, int indexA, int indexB, float radius)
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
            Vector3 p1 = _vertices[indexA];
            Vector3 p2 = _vertices[indexB];
            Vector3 middle = Vector3.Lerp(p1, p2, 0.5f).normalized * radius;

            ret = _vertices.Count;
            _vertices.Add(middle);

            // Add our new midpoint to the cache so we don't have to do this again.
            cache.Add(key, ret);
            return ret;
        }

        private void CalculateNeighbors()
        {
            foreach (MeshTriangle poly in _meshTriangles)
            {
                foreach (MeshTriangle other_poly in _meshTriangles)
                {
                    if (poly == other_poly)
                        continue;

                    if (poly.IsNeighbouring(other_poly))
                        poly.Neighbours.Add(other_poly);
                }
            }
        }

    // ================== Update ==================
        private void UpdateMesh()
        {
            Mesh planetMesh = new Mesh();
            gameObject.GetComponent<MeshRenderer>().material = _material;

            int vertexCount = _meshTriangles.Count * 3;

            int[] indices = new int[vertexCount];

            Vector3[] vertices = new Vector3[vertexCount];
            Vector3[] normals  = new Vector3[vertexCount];
            Color32[] colors   = new Color32[vertexCount];
            Vector2[] uvs      = new Vector2[vertexCount];

            for (int i = 0; i < _meshTriangles.Count; i++)
            {
                var poly = _meshTriangles[i];

                indices[i * 3 + 0] = i * 3 + 0;
                indices[i * 3 + 1] = i * 3 + 1;
                indices[i * 3 + 2] = i * 3 + 2;

                vertices[i * 3 + 0] = _vertices[poly.VertexIndices[0]];
                vertices[i * 3 + 1] = _vertices[poly.VertexIndices[1]];
                vertices[i * 3 + 2] = _vertices[poly.VertexIndices[2]];

                uvs[i * 3 + 0] = poly.UVs[0];
                uvs[i * 3 + 1] = poly.UVs[1];
                uvs[i * 3 + 2] = poly.UVs[2];

                colors[i * 3 + 0] = poly.Color;
                colors[i * 3 + 1] = poly.Color;
                colors[i * 3 + 2] = poly.Color;

                if(_shouldSmoothNormals)
                {
                    normals[i * 3 + 0] = _vertices[poly.VertexIndices[0]].normalized;
                    normals[i * 3 + 1] = _vertices[poly.VertexIndices[1]].normalized;
                    normals[i * 3 + 2] = _vertices[poly.VertexIndices[2]].normalized;
                }
                else
                {
                    Vector3 ab = _vertices[poly.VertexIndices[1]] - _vertices[poly.VertexIndices[0]];
                    Vector3 ac = _vertices[poly.VertexIndices[2]] - _vertices[poly.VertexIndices[0]];

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

            gameObject.GetComponent<MeshFilter>().mesh = planetMesh;
        }
    }
}