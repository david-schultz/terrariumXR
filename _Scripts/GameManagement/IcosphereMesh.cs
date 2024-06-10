using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR.EventSystem;
using TerrariumXR.Interaction;
using System.Linq;

namespace TerrariumXR.Geometry
{
    [RequireComponent (typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider)) ]
    public class IcosphereMesh : MonoBehaviour
    {
    // ================== References ==================
        [SerializeField] private Debugger _debugger;
        // [SerializeField] private GameStatusSO _gameSO;
        [SerializeField] private PlanetStateSO _planetStateSO;
        [SerializeField] private GrabbableGeometrySO _grabbableSO;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Color _selectionColor;
        [SerializeField] private Color _sandColor;
        [SerializeField] private Color _grassColor;
        [SerializeField] private Color _mountainColor;

    // ================== Event Channels ==================
        [SerializeField] private VoidEventChannelSO _meshUpdateChannel;
        [SerializeField] private GrabbablePositionEventChannelSO _grabbablePositionDataChannel;
        [SerializeField] private VoidEventChannelSO _grabbableMovedEventChannel;

    // ================== Variables ==================
        private Mesh _planetMesh;
        private int[] _indices;
        // private SimpleOctree _octree;
        private Vector3[] _vertices;
        private Vector3[] _normals;
        private Color32[] _colors;
        private Vector2[] _uvs;

    // ================== Event Listening ==================
        private void OnEnable()
        {
            _meshUpdateChannel.OnEventRaised += UpdateEntireMesh;
            _grabbablePositionDataChannel.OnEventRaised += UpdateGrabbablePosition;
            _grabbableMovedEventChannel.OnEventRaised += UpdateEntireMesh;
            // _grabbableMovedEventChannel.OnEventRaised += UpdateVertexPositions;


            // _vertexPositionChannel.OnEventRaised += MoveVertex;
            // _triangleSelectionChannel.OnEventRaised += SelectTriangle;
            // _feelingLuckyChannel.OnEventRaised += ImFeelingLucky;
            // _extrudeChannel.OnEventRaised += ExtrudeSelection;
            // _flattenChannel.OnEventRaised += FlattenSelection;
        }

        private void OnDisable()
        {
            _meshUpdateChannel.OnEventRaised -= UpdateEntireMesh;
            _grabbablePositionDataChannel.OnEventRaised -= UpdateGrabbablePosition;
            _grabbableMovedEventChannel.OnEventRaised += UpdateEntireMesh;
            // _grabbableMovedEventChannel.OnEventRaised += UpdateVertexPositions;


            // _vertexPositionChannel.OnEventRaised -= MoveVertex;
            // _triangleSelectionChannel.OnEventRaised -= SelectTriangle;
            // _feelingLuckyChannel.OnEventRaised -= ImFeelingLucky;
            // _extrudeChannel.OnEventRaised -= ExtrudeSelection;
            // _flattenChannel.OnEventRaised -= FlattenSelection;
        }

        private void ImFeelingLucky()
        {
            // ExtrudeSelection();
        }


    // ================== Update ==================
        private void UpdateEntireMesh()
        {
            if (_planetStateSO.Octree.Count < 1)
            {
                Debug.LogError("Planet has no vertices.");
                return;
            }
            // _debugger.Log("updating entire mesh, with count " + _planetStateSO.Octree.Count);
            _planetMesh = new Mesh();
            gameObject.GetComponent<MeshRenderer>().material = _defaultMaterial;

            int vertexCount = _planetStateSO.MeshTriangles.Count * 3;

            _indices = new int[vertexCount];

            // _octree   = new SimpleOctree(Vector3.zero, 1f);
            _vertices = new Vector3[vertexCount];
            _normals  = new Vector3[vertexCount];
            _colors   = new Color32[vertexCount];
            _uvs      = new Vector2[vertexCount];

            for (int i = 0; i < _planetStateSO.MeshTriangles.Count; i++)
            {
                MeshTriangle poly = _planetStateSO.MeshTriangles[i];

                _indices[i * 3 + 0] = i * 3 + 0;
                _indices[i * 3 + 1] = i * 3 + 1;
                _indices[i * 3 + 2] = i * 3 + 2;

                _vertices[i * 3 + 0] = _planetStateSO.Octree.Get(poly.VertexIndices[0]);
                _vertices[i * 3 + 1] = _planetStateSO.Octree.Get(poly.VertexIndices[1]);
                _vertices[i * 3 + 2] = _planetStateSO.Octree.Get(poly.VertexIndices[2]);
                // _vertices[i * 3 + 0] = planet.Vertices[poly.VertexIndices[0]];
                // _vertices[i * 3 + 1] = planet.Vertices[poly.VertexIndices[1]];
                // _vertices[i * 3 + 2] = planet.Vertices[poly.VertexIndices[2]];

                _uvs[i * 3 + 0] = poly.UVs[0];
                _uvs[i * 3 + 1] = poly.UVs[1];
                _uvs[i * 3 + 2] = poly.UVs[2];

                float avgHeight = (_vertices[i * 3 + 0].magnitude + _vertices[i * 3 + 1].magnitude + _vertices[i * 3 + 2].magnitude) / 3f;
                float minHeight = Mathf.Min(_vertices[i * 3 + 0].magnitude, _vertices[i * 3 + 1].magnitude, _vertices[i * 3 + 2].magnitude);
                
                if (avgHeight > 0.295f && minHeight > 0.275)
                {
                    _colors[i * 3 + 0] = _mountainColor;
                    _colors[i * 3 + 1] = _mountainColor;
                    _colors[i * 3 + 2] = _mountainColor;
                }
                else if (avgHeight > 0.265)
                {
                    _colors[i * 3 + 0] = _grassColor;
                    _colors[i * 3 + 1] = _grassColor;
                    _colors[i * 3 + 2] = _grassColor;
                }
                else
                {
                    _colors[i * 3 + 0] = _sandColor;
                    _colors[i * 3 + 1] = _sandColor;
                    _colors[i * 3 + 2] = _sandColor;
                }

                int sum = 0;
                for (int k = 0; k < poly.Neighbours.Count; k++)
                {
                    if (poly.Neighbours[k].Color == _grassColor)
                    {
                        sum++;
                    }
                }

                if (sum >= 2)
                {
                    _colors[i * 3 + 0] = _grassColor;
                    _colors[i * 3 + 1] = _grassColor;
                    _colors[i * 3 + 2] = _grassColor;
                }

                // for (int j = 0; j < 3; j++)
                // {
                //     if (_vertices[i * 3 + j].magnitude > 0.3f)
                //     {
                //         _colors[i * 3 + j]  = _mountainColor;
                //     }
                //     else if (_vertices[i * 3 + j].magnitude > 0.27f)
                //     {
                //         _colors[i * 3 + j]  = _grassColor;
                //     }
                //     else
                //     {
                //         _colors[i * 3 + j]  = _sandColor;
                //     }
                // }

                if(_planetStateSO.SmoothNormals)
                {
                    _normals[i * 3 + 0] = _planetStateSO.Octree.Get(poly.VertexIndices[0]).normalized;
                    _normals[i * 3 + 1] = _planetStateSO.Octree.Get(poly.VertexIndices[1]).normalized;
                    _normals[i * 3 + 2] = _planetStateSO.Octree.Get(poly.VertexIndices[2]).normalized;
                }
                else
                {
                    Vector3 ab = _planetStateSO.Octree.Get(poly.VertexIndices[1]) - _planetStateSO.Octree.Get(poly.VertexIndices[0]);
                    Vector3 ac = _planetStateSO.Octree.Get(poly.VertexIndices[2]) - _planetStateSO.Octree.Get(poly.VertexIndices[0]);

                    Vector3 normal = Vector3.Cross(ab, ac).normalized;

                    _normals[i * 3 + 0] = normal;
                    _normals[i * 3 + 1] = normal;
                    _normals[i * 3 + 2] = normal;
                }
            }

            _planetMesh.vertices = _vertices;
            _planetMesh.normals  = _normals;
            _planetMesh.colors32 = _colors;
            _planetMesh.uv       = _uvs;

            _planetMesh.SetTriangles(_indices, 0);

            gameObject.GetComponent<MeshFilter>().mesh = _planetMesh;
            gameObject.GetComponent<MeshCollider>().sharedMesh = _planetMesh;
        }

    // ================== Drag ==================
        // private void MoveVertex(int index, float delta)
        // private void MoveVertex(int index, Vector3 position)
        // {
        //     _vertices[index] = position;
        //     _planetMesh.vertices = _vertices;
        //     gameObject.GetComponent<MeshFilter>().mesh = _planetMesh;
        //     gameObject.GetComponent<MeshCollider>().sharedMesh = _planetMesh;
        // }

        private void UpdateVertexPositions()
        {
            // VertexSelection:
            // ...update the mesh's vertex at [IndexA]
            // ...using Direction * CurrentDistance from _grabbableSO.
            if (_grabbableSO.VertIds[0] != -1 && _grabbableSO.VertIds[1] == -1)
            {

                // _debugger.Log("Adjusted #" + _grabbableSO.IndexA + " to " + _planetStateSO.Octree.Get(_grabbableSO.IndexA));
                // _debugger.Log("Adjusted #" + _grabbableSO.IndexA);

                // _planetMesh.vertices = _planetStateSO.Octree.GetAll().Values.ToArray();
                _vertices[_grabbableSO.VertIds[1]] = _grabbableSO.Direction * _grabbableSO.CurrentDistance;
                _planetMesh.vertices = _vertices;

                // _planetMesh.SetTriangles(_indices, 0);

                gameObject.GetComponent<MeshFilter>().mesh = _planetMesh;
                gameObject.GetComponent<MeshCollider>().sharedMesh = _planetMesh;
            }

            // EdgeSelection:

            // TriangleSelection:




            // if (_grabbableSO.IndexA != -1 && _grabbableSO.IndexB == -1)
            // {
            //     Vector3 a = _planetStateSO.Octree.Get(_grabbableSO.IndexA);
            //     Vector3 b = _planetStateSO.Octree.Get(_grabbableSO.IndexB);
            //     Vector3 mid = (a + b) / 2f;
            //     Vector3 delta = mid - a;

            //     foreach (KeyValuePair<int, Vector3> kvp in _planetStateSO.Octree.GetAll())
            //     {
            //         Vector3 v = kvp.Value;
            //         v += delta;
            //         _planetStateSO.Octree.Adjust(kvp.Key, v);
            //     }
            // }
            
            // _vertices = ?
            // _planetMesh.vertices = _vertices;
            // gameObject.GetComponent<MeshFilter>().mesh = _planetMesh;
            // gameObject.GetComponent<MeshCollider>().sharedMesh = _planetMesh;
        }

        private void UpdateGrabbablePosition(IGrabbablePositionData data)
        {
            int[] vertices = data.vertices;
            float delta = data.delta;

            foreach (int i in vertices)
            {
                if (i != -1)
                {
                    Vector3 v = _planetStateSO.Octree.Get(i);
                    v = v.normalized * (v.magnitude + delta);
                    _planetStateSO.Octree.Adjust(i, v);
                }
                // Vector3 v = _vertices[i];
                // v = v.normalized * (v.magnitude + delta);
                // _vertices[i] = v;
            }

            //TODO: check heights, apply color
            // check if smooth normals is a good idea

            // convert dictionary to list
            List<Vector3> verts = new List<Vector3>();
            foreach (KeyValuePair<int, Vector3> kvp in _planetStateSO.Octree.GetAll())
            {
                verts.Add(kvp.Value);
            }

            _planetMesh.vertices = verts.ToArray();
            // _planetMesh.vertices = _vertices;
            gameObject.GetComponent<MeshFilter>().mesh = _planetMesh;
            gameObject.GetComponent<MeshCollider>().sharedMesh = _planetMesh;
        }



    // ================== Helpers ==================
        // /// <summary>
        // ///  Adds two MeshTriangles on each edge of the given TriangleHashSet, and moves the original
        // ///  triangles "up" by updating the referenced vertices.
        // /// </summary>
        // /// <returns>
        // ///  Returns the newly stitched "walls" in a TriangleHashSet.
        // /// </returns>
        // private TriangleHashSet StitchPolys(TriangleHashSet polys, out BorderHashSet stitchedEdge)
        // {
        //     TriangleHashSet stitchedPolys = new TriangleHashSet();

        //     stitchedPolys.IterationIndex = _gameSO.State.PlanetState.Octree.Count;
        //     // stitchedPolys.IterationIndex = _gameSO.State.PlanetState.Vertices.Count;

        //     stitchedEdge      = polys.CreateBorderHashSet();
        //     var originalVerts = stitchedEdge.ExcludeDuplicates();
        //     var newVerts      = CloneVertices(originalVerts);

        //     stitchedEdge.Separate(originalVerts, newVerts);

        //     foreach (TriangleBorder edge in stitchedEdge)
        //     {
        //         // Create new polys along the stitched edge. These
        //         // will connect the original poly to its former
        //         // neighbor.

        //         var stitch_poly1 = new MeshTriangle(edge.OuterVertices[0],
        //                                     edge.OuterVertices[1],
        //                                     edge.InnerVertices[0]);
        //         var stitch_poly2 = new MeshTriangle(edge.OuterVertices[1],
        //                                     edge.InnerVertices[1],
        //                                     edge.InnerVertices[0]);

        //         stitch_poly1.IsSide = true;
        //         stitch_poly2.IsSide = true;

        //         // Add the new stitched faces as neighbors to
        //         // the original Polys.
        //         edge.InnerTriangle.UpdateNeighbour(edge.OuterTriangle, stitch_poly2);
        //         edge.OuterTriangle.UpdateNeighbour(edge.InnerTriangle, stitch_poly1);

        //         _gameSO.State.PlanetState.MeshTriangles.Add(stitch_poly1);
        //         _gameSO.State.PlanetState.MeshTriangles.Add(stitch_poly2);

        //         stitchedPolys.Add(stitch_poly1);
        //         stitchedPolys.Add(stitch_poly2);
        //     }

        //     //Swap to the new vertices on the inner polys.
        //     foreach (MeshTriangle poly in polys)
        //     {
        //         poly.IsExtruded = true;
        //         for (int i = 0; i < 3; i++)
        //         {
        //             int vert_id = poly.VertexIndices[i];
        //             if (!originalVerts.Contains(vert_id))
        //                 continue;
        //             int vert_index = originalVerts.IndexOf(vert_id);
        //             poly.VertexIndices[i] = newVerts[vert_index];
        //         }
        //     }

        //     return stitchedPolys;
        // }

        // private TriangleHashSet RemoveSides(TriangleHashSet polys)
        // {
        //     // TriangleHashSet stitchedPolys = new TriangleHashSet();
        //     BorderHashSet stitchedEdge = polys.CreateBorderHashSet();
        //     var originalVerts = stitchedEdge.ExcludeDuplicates();
        //     var removeVerts = [...];


        //     foreach (TriangleBorder edge in stitchedEdge)
        //     {
        //         MeshTriangle outsideNeighbour = edge.InnerTriangle.Neighbour
        //     }

        // }





        // //TODO: potential bug in this adjustment?
        // private List<int> CloneVertices(List<int> oldVerts)
        // {
        //     List<int> newVerts = new List<int>();
        //     foreach (int oldVert in oldVerts)
        //     {
        //         Vector3 clonedVert = _gameSO.State.PlanetState.Octree.Get(oldVert);
        //         newVerts.Add(_gameSO.State.PlanetState.Octree.Count);
        //         _gameSO.State.PlanetState.Octree.Add(clonedVert);

        //         // Vector3 cloned_vert = _gameSO.State.PlanetState.Vertices[old_vert];
        //         // new_verts.Add(_gameSO.State.PlanetState.Vertices.Count);
        //         // _gameSO.State.PlanetState.Vertices.Add(cloned_vert);
        //     }
        //     return newVerts;
        // }

        // private List<int> GetSharedVertices(int index)
        // {
        //     List<int> verts = new List<int>();
        //     foreach (MeshTriangle poly in _gameSO.State.PlanetState.MeshTriangles)
        //     {
        //         bool contains = false;
        //         foreach (int i in poly.VertexIndices)
        //         {

        //         }
        //     }
        //     return verts;
        // }
        // // ================== Extrude ==================

        // private void ExtrudeSelection()
        // {
        //     // Sort selected polys into levels
        //     TriangleHashSet seaLevel = new TriangleHashSet();
        //     TriangleHashSet hillLevel = new TriangleHashSet();
        //     // TriangleHashSet level2 = new TriangleHashSet();

        //     foreach (int i in _gameSO.State.SelectedTriangles)
        //     {
        //         MeshTriangle poly = _gameSO.State.PlanetState.MeshTriangles[i];
        //         if (poly.ExtrudeCount == 0)
        //         {
        //             seaLevel.Add(poly);
        //             poly.ExtrudeCount++;
        //         }
        //         else if (poly.ExtrudeCount == 1)
        //         {
        //             hillLevel.Add(poly);
        //             poly.ExtrudeCount++;
        //         }
        //         // can't extrude level 2 any further
        //     }

        //     seaLevel.ApplyColor(_grassColor);
        //     TriangleHashSet sides = Extrude(seaLevel, 0.01f);
        //     sides.ApplyColor(_sandColor);

        //     hillLevel.ApplyColor(_mountainColor);
        //     sides = Extrude(hillLevel, 0.01f);
        //     sides.ApplyColor(_grassColor);

        //     // // Filter out any already extruded polys
        //     // TriangleHashSet filteredSelection = new TriangleHashSet();
        //     // TriangleHashSet insetSelection = new TriangleHashSet();
        //     // foreach (int i in _gameSO.State.SelectedTriangles)
        //     // {
        //     //     MeshTriangle poly = _gameSO.State.PlanetState.MeshTriangles[i];
        //     //     poly.ExtrudeCount++;

        //     //     if (poly.IsInset)
        //     //     {
        //     //         insetSelection.Add(poly);
        //     //         filteredSelection.Add(poly);
        //     //     }
        //     //     else if (!poly.IsExtruded && !poly.IsSide)
        //     //     {
        //     //         filteredSelection.Add(poly);
        //     //     }
        //     // }

        //     // // Flatten all inset polygons before extruding
        //     // Flatten(insetSelection);

        //     // filteredSelection.ApplyColor(_grassColor);
        //     // TriangleHashSet sides = Extrude(filteredSelection, 0.01f);
        //     // sides.ApplyColor(_sandColor);

        //     UpdateEntireMesh(_gameSO.State.PlanetState);
        // }

        // private void FlattenSelection()
        // {
        //     // Sort selected polys into levels
        //     TriangleHashSet mountainLevel = new TriangleHashSet();
        //     TriangleHashSet hillLevel = new TriangleHashSet();
        //     // TriangleHashSet seaLevel = new TriangleHashSet();

        //     foreach (int i in _gameSO.State.SelectedTriangles)
        //     {
        //         MeshTriangle poly = _gameSO.State.PlanetState.MeshTriangles[i];
        //         if (poly.ExtrudeCount == 2)
        //         {
        //             mountainLevel.Add(poly);
        //             poly.ExtrudeCount--;
        //         }
        //         else if (poly.ExtrudeCount == 1)
        //         {
        //             hillLevel.Add(poly);
        //             poly.ExtrudeCount--;
        //         }
        //         // can't flatten level 0 any further
        //     }

        //     mountainLevel.ApplyColor(_grassColor);
        //     TriangleHashSet sides = Flatten(mountainLevel);
        //     sides.ApplyColor(_sandColor);

        //     hillLevel.ApplyColor(_sandColor);
        //     sides = Flatten(hillLevel);
        //     sides.ApplyColor(_sandColor);


        //     // // Filter out any non-extruded polys
        //     // TriangleHashSet filteredSelection = new TriangleHashSet();
        //     // foreach (int i in _gameSO.State.SelectedTriangles)
        //     // {
        //     //     MeshTriangle poly = _gameSO.State.PlanetState.MeshTriangles[i];
        //     //     if ((poly.IsExtruded || poly.IsInset) && !poly.IsSide)
        //     //     {
        //     //         filteredSelection.Add(poly);
        //     //     }
        //     // }

        //     // filteredSelection.ApplyColor(FindColor("GrassColor"));
        //     // TriangleHashSet flattened = Flatten(filteredSelection);
        //     // sides.ApplyColor(FindColor("DirtColor"));

        //     UpdateEntireMesh(_gameSO.State.PlanetState);
        // }

        // private void PullExtrusion()
        // {

        // }

        // private void ClearExtrusion()
        // {
            
        // }

    // // ================== Extrude ==================
    //     private TriangleHashSet Extrude(TriangleHashSet polys, float height)
    //     {
    //         BorderHashSet stitchedEdge;
    //         TriangleHashSet stitchedPolys = StitchPolys(polys, out stitchedEdge);
    //         List<int> verts = polys.ExcludeDuplicates();

    //         // Take each vertex in this list of polys, and push it
    //         // away from the center of the Planet by the height
    //         // parameter.

    //         foreach (int vert in verts)
    //         {
    //             Vector3 v = _gameSO.State.PlanetState.Vertices[vert];
    //             v = v.normalized * (v.magnitude + height);
    //             _gameSO.State.PlanetState.Vertices[vert] = v;
    //         }

    //         // Make sure to update the MeshTriangle data.
    //         foreach (MeshTriangle poly in polys)
    //         {
    //             poly.ExtrudeDelta = height;
    //         }

    //         return stitchedPolys;
    //     }

    //     private TriangleHashSet Inset(TriangleHashSet polys, float insetDistance)
    //     {
    //         BorderHashSet stitchedEdge;
    //         TriangleHashSet stitchedPolys = StitchPolys(polys, out stitchedEdge);

    //         Dictionary<int, Vector3> inwardDirections = stitchedEdge.GetInwardDirections(_gameSO.State.PlanetState.Vertices);

    //         // Push each vertex inwards, then correct
    //         // it's height so that it's as far from the center of
    //         // the planet as it was before.

    //         foreach (KeyValuePair<int, Vector3> kvp in inwardDirections)
    //         {
    //             int     vertIndex       = kvp.Key;
    //             Vector3 inwardDirection = kvp.Value;

    //             Vector3 vertex = _gameSO.State.PlanetState.Vertices[vertIndex];
    //             float originalHeight = vertex.magnitude;

    //             vertex += inwardDirection * insetDistance;
    //             vertex  = vertex.normalized * originalHeight;
    //             _gameSO.State.PlanetState.Vertices[vertIndex] = vertex;
    //         }

    //         return stitchedPolys;
    //     }

    //     private TriangleHashSet Flatten(TriangleHashSet polys)
    //     {
    //         // delete all vertices and mesh triangles that are IsSide
    //         // set polys vertex height back to 0

    //         // polys = RemoveSides(polys);

    //         List<int> verts = polys.ExcludeDuplicates();
    //         foreach (int vert in verts)
    //         {
    //             Vector3 v = _gameSO.State.PlanetState.Vertices[vert];

    //             float avgExtrudeDelta = GetAvgExtrudeDelta(vert);
                
    //             v = v.normalized * (v.magnitude - avgExtrudeDelta);
    //             _gameSO.State.PlanetState.Vertices[vert] = v;
    //         }

    //         // Make sure to update the MeshTriangle data.
    //         foreach (MeshTriangle poly in polys)
    //         {
    //             poly.ExtrudeDelta = 0f;
    //         }

    //         return new TriangleHashSet();
    //     }

    //     private float GetAvgExtrudeDelta(int vert)
    //     {
    //         float avg = 0;
    //         int count = 0;
    //         // Check each meshtriangle's 
    //         foreach (MeshTriangle poly in _gameSO.State.PlanetState.MeshTriangles)
    //         {
    //             if (poly.VertexIndices.Contains(vert))
    //             {
    //                 avg += poly.ExtrudeDelta;
    //                 count++;
    //             }
    //         }

    //         if (count > 0)
    //         {
    //             return avg / count;
    //         }
    //         else
    //         {
    //             return 0;
    //         }
    //     }

    // // ================== Select ==================
    //     private void SelectTriangle(int index)
    //     {
    //         MeshTriangle poly = _gameSO.State.PlanetState.MeshTriangles[index];

    //         if (poly.IsSelected)
    //         {
    //             // _debugger.Log(index + "poly selected");
    //             _colors[index * 3 + 0] = _selectionColor;
    //             _colors[index * 3 + 1] = _selectionColor;
    //             _colors[index * 3 + 2] = _selectionColor;
    //         }
    //         else
    //         {
    //             _colors[index * 3 + 0] = poly.Color;
    //             _colors[index * 3 + 1] = poly.Color;
    //             _colors[index * 3 + 2] = poly.Color;
    //         }

    //         _planetMesh.colors32 = _colors;
    //         gameObject.GetComponent<MeshFilter>().mesh = _planetMesh;
    //         gameObject.GetComponent<MeshCollider>().sharedMesh = _planetMesh;
    //     }

    }
}