// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TerrariumXR.EventSystem;

// namespace TerrariumXR.Geometry
// {
//     // public class IcosphereData
//     // {
//     //     public int[] indices;
//     //     public Vector3[] vertices;
//     //     public Vector3[] normals;
//     //     public Color32[] colors;
//     //     public Vector2[] uvs;
//     //     public MeshTriangle[] triangles;

//     //     public IcosphereData(PlanetState planetState)
//     //     {

//     //         int vertexCount = planet.MeshTriangles.Count * 3;
//     //         indices   = new int[vertexCount];
//     //         vertices  = new Vector3[vertexCount];
//     //         normals   = new Vector3[vertexCount];
//     //         colors    = new Color32[vertexCount];
//     //         uvs       = new Vector2[vertexCount];
//     //         triangles = new MeshTriangle[planet.MeshTriangles.Count]
//     //     }

//     //     public IcosphereData(int[] i, Vector3[] v, Vector3[] n, Color32[] c, Vector2[] u)
//     //     {
//     //         indices  = i;
//     //         vertices = v;
//     //         normals  = n;
//     //         colors   = c;
//     //         uvs      = u;
//     //     }
//     // }

//     /// <summary>
//     /// Used by creating a new IcosphereEditor(), then call function (e.g. Extrude()) which returns a modified version of xyz.
//     /// </summary>
//     public class IcosphereEditor
//     {
//     // ================== Variables ==================
//         private List<MeshTriangle> _meshTriangles;
//         private List<Vector3> _vertices;

//     // ================== Variables ==================
//         // private IcosphereData _ico;
//         // private Mesh _planetMesh;
//         // private int[] _indices;
//         // private Vector3[] _vertices;
//         // private Vector3[] _normals;
//         // private Color32[] _colors;
//         // private Vector2[] _uvs;

//         public IcosphereEditor(PlanetState planetState)
//         {
//             _meshTriangles = planetState.MeshTriangles;
//             _vertices = planetState.Vertices;
//         }

//         public IcosphereEditor(GameStatusSO gameSO)
//         {
//             _meshTriangles = gameSO.State.PlanetState.MeshTriangles;
//             _vertices = gameSO.State.PlanetState.Vertices;
//         }

//     // // ================== Public Functions ==================


//     // // ================== Private Functions ==================
//     //     private TriangleHashSet Extrude(TriangleHashSet polys, float height)
//     //     {
//     //         BorderHashSet stitchedEdge;
//     //         TriangleHashSet stitchedPolys = StitchPolys(polys, out stitchedEdge);
//     //         List<int> verts = polys.ExcludeDuplicates();

//     //         // Take each vertex in this list of polys, and push it
//     //         // away from the center of the Planet by the height
//     //         // parameter.

//     //         foreach (int vert in verts)
//     //         {
//     //             Vector3 v = _vertices[vert];
//     //             v = v.normalized * (v.magnitude + height);
//     //             _vertices[vert] = v;
//     //         }

//     //         return stitchedPolys;
//     //     }

//     //     private TriangleHashSet Inset(TriangleHashSet polys, float insetDistance)
//     //     {
//     //         BorderHashSet stitchedEdge;
//     //         TriangleHashSet stitchedPolys = StitchPolys(polys, out stitchedEdge);

//     //         Dictionary<int, Vector3> inwardDirections = stitchedEdge.GetInwardDirections(_vertices);

//     //         // Push each vertex inwards, then correct
//     //         // it's height so that it's as far from the center of
//     //         // the planet as it was before.

//     //         foreach (KeyValuePair<int, Vector3> kvp in inwardDirections)
//     //         {
//     //             int     vertIndex       = kvp.Key;
//     //             Vector3 inwardDirection = kvp.Value;

//     //             Vector3 vertex = _vertices[vertIndex];
//     //             float originalHeight = vertex.magnitude;

//     //             vertex += inwardDirection * insetDistance;
//     //             vertex  = vertex.normalized * originalHeight;
//     //             _vertices[vertIndex] = vertex;
//     //         }

//     //         return stitchedPolys;
//     //     }

//     //     private void Subdivide(int subdivisions)
//     //     {
//     //         var midPointCache = new Dictionary<int, int>();

//     //         for (int i = 0; i < subdivisions; i++)
//     //         {
//     //             var newPolys = new List<MeshTriangle>();
//     //             foreach (var poly in _meshTriangles)
//     //             {
//     //                 int a = poly.VertexIndices[0];
//     //                 int b = poly.VertexIndices[1];
//     //                 int c = poly.VertexIndices[2];

//     //                 int ab = GetMidPointIndex(midPointCache, a, b);
//     //                 int bc = GetMidPointIndex(midPointCache, b, c);
//     //                 int ca = GetMidPointIndex(midPointCache, c, a);

//     //                 newPolys.Add(new MeshTriangle(a, ab, ca));
//     //                 newPolys.Add(new MeshTriangle(b, bc, ab));
//     //                 newPolys.Add(new MeshTriangle(c, ca, bc));
//     //                 newPolys.Add(new MeshTriangle(ab, bc, ca));
//     //             }
                
//     //             _meshTriangles = newPolys;
//     //         }
//     //     }

//     //     private int GetMidPointIndex(Dictionary<int, int> cache, int indexA, int indexB)
//     //     {
//     //         int smallerIndex = Mathf.Min(indexA, indexB);
//     //         int greaterIndex = Mathf.Max(indexA, indexB);
//     //         int key = (smallerIndex << 16) + greaterIndex;

//     //         // If a midpoint is already defined, just return it.

//     //         int ret;
//     //         if (cache.TryGetValue(key, out ret))
//     //             return ret;

//     //         // If we're here, it's because a midpoint for these two
//     //         // _vertices hasn't been created yet. Let's do that now!

//     //         Vector3 p1 = _vertices[indexA];
//     //         Vector3 p2 = _vertices[indexB];
//     //         Vector3 middle = Vector3.Lerp(p1, p2, 0.5f).normalized;

//     //         ret = _vertices.Count;
//     //         _vertices.Add(middle);

//     //         // Add our new midpoint to the cache so we don't have
//     //         // to do this again. =)

//     //         cache.Add(key, ret);
//     //         return ret;
//     //     }

//     //     private void CalculateNeighbors()
//     //     {
//     //         foreach (MeshTriangle poly in _meshTriangles)
//     //         {
//     //             foreach (MeshTriangle other_poly in _meshTriangles)
//     //             {
//     //                 if (poly == other_poly)
//     //                     continue;

//     //                 if (poly.IsNeighbouring(other_poly))
//     //                     poly.Neighbours.Add(other_poly);
//     //             }
//     //         }
//     //     }

//     //     private List<int> CloneVertices(List<int> old_verts)
//     //     {
//     //         List<int> new_verts = new List<int>();
//     //         foreach (int old_vert in old_verts)
//     //         {
//     //             Vector3 cloned_vert = _vertices[old_vert];
//     //             new_verts.Add(_vertices.Count);
//     //             _vertices.Add(cloned_vert);
//     //         }
//     //         return new_verts;
//     //     }

//     //     private TriangleHashSet StitchPolys(TriangleHashSet polys, out BorderHashSet stitchedEdge)
//     //     {
//     //         TriangleHashSet stichedPolys = new TriangleHashSet();

//     //         stichedPolys.IterationIndex = _vertices.Count;

//     //         stitchedEdge      = polys.CreateBorderHashSet();
//     //         var originalVerts = stitchedEdge.ExcludeDuplicates();
//     //         var newVerts      = CloneVertices(originalVerts);

//     //         stitchedEdge.Separate(originalVerts, newVerts);

//     //         foreach (TriangleBorder edge in stitchedEdge)
//     //         {
//     //             // Create new polys along the stitched edge. These
//     //             // will connect the original poly to its former
//     //             // neighbor.

//     //             var stitch_poly1 = new MeshTriangle(edge.OuterVertices[0],
//     //                                         edge.OuterVertices[1],
//     //                                         edge.InnerVertices[0]);
//     //             var stitch_poly2 = new MeshTriangle(edge.OuterVertices[1],
//     //                                         edge.InnerVertices[1],
//     //                                         edge.InnerVertices[0]);
//     //             // Add the new stitched faces as neighbors to
//     //             // the original Polys.
//     //             edge.InnerTriangle.UpdateNeighbour(edge.OuterTriangle, stitch_poly2);
//     //             edge.OuterTriangle.UpdateNeighbour(edge.InnerTriangle, stitch_poly1);

//     //             _meshTriangles.Add(stitch_poly1);
//     //             _meshTriangles.Add(stitch_poly2);

//     //             stichedPolys.Add(stitch_poly1);
//     //             stichedPolys.Add(stitch_poly2);
//     //         }

//     //         //Swap to the new _vertices on the inner polys.
//     //         foreach (MeshTriangle poly in polys)
//     //         {
//     //             for (int i = 0; i < 3; i++)
//     //             {
//     //                 int vert_id = poly.VertexIndices[i];
//     //                 if (!originalVerts.Contains(vert_id))
//     //                     continue;
//     //                 int vert_index = originalVerts.IndexOf(vert_id);
//     //                 poly.VertexIndices[i] = newVerts[vert_index];
//     //             }
//     //         }

//     //         return stichedPolys;
//     //     }

//     //     private TriangleHashSet GetTriangles(Vector3 center, float radius, IEnumerable<MeshTriangle> source)
//     //     {
//     //         TriangleHashSet newSet = new TriangleHashSet();

//     //         foreach(MeshTriangle p in source)
//     //         {
//     //             foreach(int vertexIndex in p.VertexIndices)
//     //             {
//     //                 float distanceToSphere = Vector3.Distance(center, _vertices[vertexIndex]);

//     //                 if (distanceToSphere <= radius)
//     //                 {
//     //                     newSet.Add(p);
//     //                     break;
//     //                 }
//     //             }
//     //         }

//     //         return newSet;
//     //     }

//     //     private void CreateMesh(PlanetState planet)
//     //     {
//     //         _planetMesh = new Mesh();
//     //         gameObject.GetComponent<MeshRenderer>().material = _defaultMaterial;

//     //         int vertexCount = planet.MeshTriangles.Count * 3;

//     //         _indices = new int[vertexCount];

//     //         _vertices = new Vector3[vertexCount];
//     //         _normals  = new Vector3[vertexCount];
//     //         _colors   = new Color32[vertexCount];
//     //         _uvs      = new Vector2[vertexCount];

//     //         for (int i = 0; i < planet.MeshTriangles.Count; i++)
//     //         {
//     //             MeshTriangle poly = planet.MeshTriangles[i];

//     //             _indices[i * 3 + 0] = i * 3 + 0;
//     //             _indices[i * 3 + 1] = i * 3 + 1;
//     //             _indices[i * 3 + 2] = i * 3 + 2;

//     //             _vertices[i * 3 + 0] = planet.Vertices[poly.VertexIndices[0]];
//     //             _vertices[i * 3 + 1] = planet.Vertices[poly.VertexIndices[1]];
//     //             _vertices[i * 3 + 2] = planet.Vertices[poly.VertexIndices[2]];

//     //             _uvs[i * 3 + 0] = poly.UVs[0];
//     //             _uvs[i * 3 + 1] = poly.UVs[1];
//     //             _uvs[i * 3 + 2] = poly.UVs[2];

//     //             if (poly.IsSelected)
//     //             {
//     //                 // _debugger.Log(i + "poly selected");
//     //                 _colors[i * 3 + 0] = _selectionColor;
//     //                 _colors[i * 3 + 1] = _selectionColor;
//     //                 _colors[i * 3 + 2] = _selectionColor;
//     //             }
//     //             else
//     //             {
//     //                 _colors[i * 3 + 0] = poly.Color;
//     //                 _colors[i * 3 + 1] = poly.Color;
//     //                 _colors[i * 3 + 2] = poly.Color;
//     //             }

//     //             if(planet.SmoothNormals)
//     //             {
//     //                 _normals[i * 3 + 0] = planet.Vertices[poly.VertexIndices[0]].normalized;
//     //                 _normals[i * 3 + 1] = planet.Vertices[poly.VertexIndices[1]].normalized;
//     //                 _normals[i * 3 + 2] = planet.Vertices[poly.VertexIndices[2]].normalized;
//     //             }
//     //             else
//     //             {
//     //                 Vector3 ab = planet.Vertices[poly.VertexIndices[1]] - planet.Vertices[poly.VertexIndices[0]];
//     //                 Vector3 ac = planet.Vertices[poly.VertexIndices[2]] - planet.Vertices[poly.VertexIndices[0]];

//     //                 Vector3 normal = Vector3.Cross(ab, ac).normalized;

//     //                 _normals[i * 3 + 0] = normal;
//     //                 _normals[i * 3 + 1] = normal;
//     //                 _normals[i * 3 + 2] = normal;
//     //             }
//     //         }

//     //         _planetMesh.vertices = _vertices;
//     //         _planetMesh.normals  = _normals;
//     //         _planetMesh.colors32 = _colors;
//     //         _planetMesh.uv       = _uvs;

//     //         _planetMesh.SetTriangles(_indices, 0);

//     //         gameObject.GetComponent<MeshFilter>().mesh = _planetMesh;
//     //         gameObject.GetComponent<MeshCollider>().sharedMesh = _planetMesh;
//     //     }
//     }
// }