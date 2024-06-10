using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR.EventSystem;

namespace TerrariumXR.Geometry
{
    public class IcosphereGenerator
    {
    // ================== Variables ==================
        private SimpleOctree _octree;
        private List<MeshTriangle> _meshTriangles;

        public IcosphereGenerator(int subdivisions, float radius)
        {
            _octree = new SimpleOctree(Vector3.zero, 1f);
            _meshTriangles = new List<MeshTriangle>();
            
            MakeIcosphere(radius);
            Subdivide(subdivisions, radius);
            CalculateNeighbors();
        }

        public IcosphereGenerator(int subdivisions, float radius, Color32 defaultColor)
        {
            _octree = new SimpleOctree(Vector3.zero, 1f);
            _meshTriangles = new List<MeshTriangle>();
            
            MakeIcosphere(radius);
            Subdivide(subdivisions, radius);
            CalculateNeighbors();
            ApplyDefaultColor(defaultColor);
        }

        public SimpleOctree GetOctree()
        {
            return _octree;
        }

        public List<MeshTriangle> GetMeshTriangles()
        {
            return _meshTriangles;
        }

    // ================== Generation ==================
        private void MakeIcosphere(float radius)
        {
            float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

            _octree.Add(new Vector3(-1,  t,  0).normalized * radius); //0
            _octree.Add(new Vector3( 1,  t,  0).normalized * radius); //1
            _octree.Add(new Vector3(-1, -t,  0).normalized * radius); //2
            _octree.Add(new Vector3( 1, -t,  0).normalized * radius); //3
            _octree.Add(new Vector3( 0, -1,  t).normalized * radius); //4
            _octree.Add(new Vector3( 0,  1,  t).normalized * radius); //5
            _octree.Add(new Vector3( 0, -1, -t).normalized * radius); //6
            _octree.Add(new Vector3( 0,  1, -t).normalized * radius); //7
            _octree.Add(new Vector3( t,  0, -1).normalized * radius); //8
            _octree.Add(new Vector3( t,  0,  1).normalized * radius); //9
            _octree.Add(new Vector3(-t,  0, -1).normalized * radius); //10
            _octree.Add(new Vector3(-t,  0,  1).normalized * radius); //11

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

            // _octree.Add(0,  ConverterExtensions.ToSystem(new Vector3(-1,  t,  0).normalized * radius));
            // _octree.Add(1,  ConverterExtensions.ToSystem(new Vector3( 1,  t,  0).normalized * radius));
            // _octree.Add(2,  ConverterExtensions.ToSystem(new Vector3(-1, -t,  0).normalized * radius));
            // _octree.Add(3,  ConverterExtensions.ToSystem(new Vector3( 1, -t,  0).normalized * radius));
            // _octree.Add(4,  ConverterExtensions.ToSystem(new Vector3( 0, -1,  t).normalized * radius));
            // _octree.Add(5,  ConverterExtensions.ToSystem(new Vector3( 0,  1,  t).normalized * radius));
            // _octree.Add(6,  ConverterExtensions.ToSystem(new Vector3( 0, -1, -t).normalized * radius));
            // _octree.Add(7,  ConverterExtensions.ToSystem(new Vector3( 0,  1, -t).normalized * radius));
            // _octree.Add(8,  ConverterExtensions.ToSystem(new Vector3( t,  0, -1).normalized * radius));
            // _octree.Add(9,  ConverterExtensions.ToSystem(new Vector3( t,  0,  1).normalized * radius));
            // _octree.Add(10, ConverterExtensions.ToSystem(new Vector3(-t,  0, -1).normalized * radius));
            // _octree.Add(11, ConverterExtensions.ToSystem(new Vector3(-t,  0,  1).normalized * radius));
            
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

                    int ab = AddMidPointIndex(midPointCache, a, b, radius);
                    int bc = AddMidPointIndex(midPointCache, b, c, radius);
                    int ca = AddMidPointIndex(midPointCache, c, a, radius);

                    newPolys.Add(new MeshTriangle(a, ab, ca));
                    newPolys.Add(new MeshTriangle(b, bc, ab));
                    newPolys.Add(new MeshTriangle(c, ca, bc));
                    newPolys.Add(new MeshTriangle(ab, bc, ca));
                }
                
                _meshTriangles = newPolys;
            }
        }

        private int AddMidPointIndex(Dictionary<int, int> cache, int indexA, int indexB, float radius)
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
            Vector3 p1 = _octree.Get(indexA);
            Vector3 p2 = _octree.Get(indexB);
            // Vector3 p1 = _vertices[indexA];
            // Vector3 p2 = _vertices[indexB];
            Vector3 middle = Vector3.Lerp(p1, p2, 0.5f).normalized * radius;

            ret = _octree.Add(middle);
            // _vertices.Add(middle);

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

        // public TriangleHashSet GetTriangles(Vector3 center, float radius, IEnumerable<MeshTriangle> source)
        // {
        //     TriangleHashSet newSet = new TriangleHashSet();

        //     foreach(MeshTriangle p in source)
        //     {
        //         foreach(int vertexIndex in p.VertexIndices)
        //         {
        //             float distanceToSphere = Vector3.Distance(center, Vertices[vertexIndex]);

        //             if (distanceToSphere <= radius)
        //             {
        //                 newSet.Add(p);
        //                 break;
        //             }
        //         }
        //     }

        //     return newSet;
        // }

        private void ApplyDefaultColor(Color32 color)
        {
            TriangleHashSet triangles = new TriangleHashSet();

            foreach(MeshTriangle poly in _meshTriangles)
            {
                poly.Color = color;
                triangles.Add(poly);
            }

            triangles.ApplyColor(color);
        }
    }
}