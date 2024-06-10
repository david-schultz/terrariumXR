using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;
using TerrariumXR.Geometry;
using TerrariumXR.EventSystem;
using System.Linq;
using Unity.Burst.Intrinsics;
// using System.Numerics;
// using System.Numerics;

namespace TerrariumXR.Interaction
{
    public class GeometryGrabInteraction : MonoBehaviour
    {
    // ================== References ==================
        [SerializeField] private Debugger _debugger;
        [SerializeField] private PlanetStateSO _planetStateSO;
        [SerializeField] private HandStateSO _handStateSO;
        [SerializeField] private GrabbableGeometrySO _grabbableSO;
        // [SerializeField] private GrabbablePrefabManager _prefabManager;
        [SerializeField] private GameObject _grabbableObject;
        [SerializeField] private Transform _rootPosition;

    // ================== Variables ==================
        private HashSet<int> _curOctants;
        private Dictionary<int, Vector3> _watchedVertices;
        private Vector3 _lastHandPosition = Vector3.zero;
        private const float _epsilon = 0.0001f;
        // private string _prevActivePose = "n/a";

        // private int _debugCounter = 0;

    // ================== Track current octants ==================
        [SerializeField] private IntBoolEventChannelSO _octantCollisionChannel;

        void Start()
        {
            _curOctants = new HashSet<int>();
            _watchedVertices = new Dictionary<int, Vector3>();
            // _watchedVertices = new List<Vector3>();
        }

        private void OnEnable()
        {
            _octantCollisionChannel.OnEventRaised += OctantCollision;
        }

        private void OnDisable()
        {
            _octantCollisionChannel.OnEventRaised -= OctantCollision;
        }

        private void OctantCollision(int octant, bool isColliding)
        {
            if (isColliding)
            {
                _curOctants.Add(octant);
            }
            else
            {
                _curOctants.Remove(octant);
            }

            UpdateWatchedVertices(octant, isColliding);
            // TryClear();
        }

        private void UpdateWatchedVertices(int octant, bool isColliding)
        {
            // TODO: There is a bug where it will remove vertices on the seams, because they are part of the octant.
            // ...maybe this can be addressed by simply checking if the vertex is along an axis?

            // TODO: Another bug; a vertex may get removed, even while its being grabbed.

            // _debugger.Log(_planetStateSO.Octree.GetOctant(octant).Keys.Count + " vertices in octant #" + octant);
            if (isColliding)
            {
                foreach (KeyValuePair<int, Vector3> kvp in _planetStateSO.Octree.GetOctant(octant))
                {
                    _watchedVertices.TryAdd(kvp.Key, kvp.Value);
                }
            }
            else
            {
                foreach (KeyValuePair<int, Vector3> kvp in _planetStateSO.Octree.GetOctant(octant))
                {
                    // Check whether the current vertex is in any other octant.
                    // ...if it is not, remove it from the watched vertices.
                    List<int> overlap = _planetStateSO.Octree.CalculateOctants(kvp.Value);
                    bool isSelfContained = true;
                    foreach (int oct in overlap)
                    {
                        if (_curOctants.Contains(oct))
                        {
                            isSelfContained = false;
                            break;
                        }
                    }

                    if (isSelfContained)
                    {
                        _watchedVertices.Remove(kvp.Key);
                    }
                }
            }

            // _debugger.Log("Total watched: " + _watchedVertices.Keys.Count);

                    // if (kvp.Value.x > _epsilon && kvp.Value.y > _epsilon && kvp.Value.z > _epsilon)
                    //     _watchedVertices.Remove(kvp.Key);
            
            
            // foreach (Vector3 vertex in _planetStateSO.Octree.GetOctant(octant))
            // {
            //     if (isColliding) _watchedVertices.Add(vertex);
            //     else  _watchedVertices.Remove(vertex);
            // }
        }

    // ================== Functions ==================

        private void TryClear()
        {
            if (!_grabbableSO.IsGrabbed && _curOctants.Count == 0 && _grabbableObject.activeSelf)
            {
                // _debugger.Log("TryClear() was successful");
                _curOctants.Clear();
                _watchedVertices.Clear();
                _grabbableObject.SetActive(false);
                _grabbableSO.Reset();
            }
        }

        
        // TODO: Handle case when the alt selector has changed during collision.
        // if (_prevActivePose != _handStateSO.AltHand.ActivePose) 
        void Update()
        {
            // If the current pose is invalid, return;
            if (_handStateSO.AltHand.ActivePose == "n/a") return;

            // If there are no octants to check
            // ...set grabbable to inactive
            // ...then return;
            TryClear();

            // If not grabbed:
            if (!_grabbableSO.IsGrabbed)
            {
                // First, check if hand position has significantly changed,
                // ...if not, return;
                // ...otherwise, update last hand position.
                if (Vector3.Distance(_lastHandPosition, _handStateSO.PrimaryHand.Position) < 0.005f) return;
                _lastHandPosition = _handStateSO.PrimaryHand.Position;

                // Second, define nearest
                // ...check if it has changed
                // ...if not, return;
                Vector3 nearest = CalculateNearest(_handStateSO.PrimaryHand.Position);
                if (nearest == _grabbableSO.PrevNearest) return;
                // _debugger.Log("Nearest vertex: " + nearest.ToString());

                // Then, set grabbableObject to active
                // ...update grabbableObject with new position
                // ...update grabbableSO with new position
                // ...update grabbableSO's prevNearest
                _grabbableObject.SetActive(true);
                // _grabbableObject.transform.position = transform.position;
                // _grabbableObject.transform.rotation = Quaternion.LookRotation(nearest.normalized, Vector3.up);
                // _grabbableObject.transform.Translate(nearest);
                _grabbableObject.transform.position = transform.position;
                _grabbableObject.transform.Translate(nearest);
                _grabbableSO.Direction = nearest.normalized;
                _grabbableSO.CurrentDistance = nearest.magnitude;
                _grabbableSO.PrevNearest = nearest;
            }
            else
            {
                MoveVertices();
            }
        }

        private void MoveVertices()
        {
            for (int i = 0; i < 3; i++)
            {
                int id = _grabbableSO.VertIds[i];
                if (id == -1) break;

                float initial = _grabbableSO.InitialVertDist[i];
                // _debugger.Log("Initial distance for " + id + ": " + initial);
                float distance = initial + (_grabbableSO.CurrentDistance - _grabbableSO.StandardDistance);
                distance = Mathf.Clamp(distance, _grabbableSO.MinDistance, _grabbableSO.MaxDistance);

                _grabbableSO.VertDist[i] = distance;
                // float distance = vertPos[i].magnitude + (_grabbableSO.CurrentDistance - _grabbableSO.StandardDistance);

                Vector3 position = _planetStateSO.Octree.Get(id).normalized * distance;
                _planetStateSO.Octree.Adjust(id, position);
            }

            // _planetStateSO.Octree.Adjust(_grabbableSO.IndexA, _grabbableSO.Direction * _grabbableSO.CurrentDistance);
        }



        // TODO: inside, update the current indices inside _grabbableSO.
        private Vector3 CalculateNearest(Vector3 pos)
        {
            // convert position to local space
            pos = transform.InverseTransformPoint(pos);

            List<int> seen = new List<int>();
            Tuple<int, Vector3> kvp0 = Calc(pos, seen);
            Tuple<int, Vector3> kvp1 = Calc(pos, seen);
            Tuple<int, Vector3> kvp2 = Calc(pos, seen);
            
            if (_handStateSO.AltHand.ActivePose == "VertexSelector")
            {
                _grabbableSO.VertIds[0] = kvp0.Item1;
                _grabbableSO.InitialVertDist[0] = kvp0.Item2.magnitude;
                _grabbableSO.VertIds[1] = -1;
                _grabbableSO.VertIds[2] = -1;
                return kvp0.Item2;
            }
            else if (_handStateSO.AltHand.ActivePose == "EdgeSelector")
            {
                _grabbableSO.VertIds[0] = kvp0.Item1;
                _grabbableSO.VertIds[1] = kvp1.Item1;
                _grabbableSO.InitialVertDist[0] = kvp0.Item2.magnitude;
                _grabbableSO.InitialVertDist[1] = kvp1.Item2.magnitude;
                _grabbableSO.VertIds[2] = -1;
                return (kvp0.Item2 + kvp1.Item2) / 2;
            }
            else if (_handStateSO.AltHand.ActivePose == "TriangleSelector")
            {
                _grabbableSO.VertIds[0] = kvp0.Item1;
                _grabbableSO.VertIds[1] = kvp1.Item1;
                _grabbableSO.VertIds[2] = kvp2.Item1;
                _grabbableSO.InitialVertDist[0] = kvp0.Item2.magnitude;
                _grabbableSO.InitialVertDist[1] = kvp1.Item2.magnitude;
                _grabbableSO.InitialVertDist[2] = kvp2.Item2.magnitude;
                return (kvp0.Item2 + kvp1.Item2 + kvp2.Item2) / 3;
            }

            return Vector3.zero;
        }

        private Tuple<int, Vector3> Calc(Vector3 pos, List<int> prev)
        {
            int id = -1;
            Vector3 nearest = Vector3.zero;
            float minDistanceSqr = Mathf.Infinity;

            foreach (KeyValuePair<int, Vector3> kvp in _watchedVertices)
            {
                Vector3 diff = pos - kvp.Value;
                float distSqr = diff.sqrMagnitude;

                if (distSqr < minDistanceSqr && !prev.Contains(kvp.Key))
                {
                    minDistanceSqr = distSqr;
                    nearest = kvp.Value;
                    id = kvp.Key;
                }
            }

            prev.Add(id);
            return Tuple.Create(id, nearest);
        }

    // ================== Helpers ==================
        public Vector3 GetMidPoint(Vector3 v1, Vector3 v2)
        {
            float x = ( v1.x +
                        v2.x ) / 2f;
            float y = ( v1.y +
                        v2.y ) / 2f;
            float z = ( v1.z +
                        v2.z ) / 2f;

            return new Vector3(x, y, z);
        }

        public Vector3 GetMidPoint(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            float x = ( v1.x +
                        v2.x + 
                        v3.x ) / 3f;
            float y = ( v1.y +
                        v2.y + 
                        v3.y ) / 3f;
            float z = ( v1.z +
                        v2.z + 
                        v3.z ) / 3f;

            return new Vector3(x, y, z);
        }

        // private Vector3 CalculateNearestVertex(Vector3 pos)
        // {
        //     Vector3 nearest = Vector3.zero;
        //     float minDistanceSqr = Mathf.Infinity;

        //     foreach (KeyValuePair<int, Vector3> kvp in _watchedVertices)
        //     {
        //         Vector3 diff = pos - kvp.Value;
        //         float distSqr = diff.sqrMagnitude;

        //         if (distSqr < minDistanceSqr)
        //         {
        //             minDistanceSqr = distSqr;
        //             nearest = kvp.Value;

        //             // THIS LINE IS IMPORTANT
        //             _grabbableSO.IndexA = kvp.Key;
        //             _grabbableSO.InitialADistance = kvp.Value.magnitude;
        //         }
        //     }

        //     return nearest;
        // }

        // private Vector3 CalculateNearestEdge(Vector3 pos)
        // {
        //     Vector3 nearestA = Vector3.zero;
        //     Vector3 nearestB = Vector3.zero;
        //     float minDistanceSqrA = Mathf.Infinity;
        //     float minDistanceSqrB = Mathf.Infinity;

        //     foreach (KeyValuePair<int, Vector3> kvp in _watchedVertices)
        //     {
        //         Vector3 diff = pos - kvp.Value;
        //         float distSqr = diff.sqrMagnitude;

        //         if (distSqr < minDistanceSqrA)
        //         {
        //             minDistanceSqrA = distSqr;
        //             nearestA = kvp.Value;

        //             // THIS LINE IS IMPORTANT
        //             _grabbableSO.IndexA = kvp.Key;
        //             _grabbableSO.InitialADistance = kvp.Value.magnitude;
        //         }
        //     }

        //     foreach (KeyValuePair<int, Vector3> kvp in _watchedVertices)
        //     {
        //         Vector3 diff = pos - kvp.Value;
        //         float distSqr = diff.sqrMagnitude;

        //         if (distSqr < minDistanceSqrB && nearestA != kvp.Value)
        //         {
        //             minDistanceSqrB = distSqr;
        //             nearestB = kvp.Value;

        //             // THIS LINE IS IMPORTANT
        //             _grabbableSO.IndexB = kvp.Key;
        //         }
        //     }

        //     Vector3 nearest = (nearestA + nearestB) / 2;
        //     return nearest;
        // }

        // private Vector3 CalculateNearestTriangle(Vector3 pos)
        // {
        //     Vector3 nearestA = Vector3.zero;
        //     Vector3 nearestB = Vector3.zero;
        //     Vector3 nearestC = Vector3.zero;
        //     float minDistanceSqrA = Mathf.Infinity;
        //     float minDistanceSqrB = Mathf.Infinity;
        //     float minDistanceSqrC = Mathf.Infinity;

        //     foreach (KeyValuePair<int, Vector3> kvp in _watchedVertices)
        //     {
        //         Vector3 diff = pos - kvp.Value;
        //         float distSqr = diff.sqrMagnitude;

        //         if (distSqr < minDistanceSqrA)
        //         {
        //             minDistanceSqrA = distSqr;
        //             nearestA = kvp.Value;

        //             // THIS LINE IS IMPORTANT
        //             _grabbableSO.IndexA = kvp.Key;
        //         }
        //     }

        //     foreach (KeyValuePair<int, Vector3> kvp in _watchedVertices)
        //     {
        //         Vector3 diff = pos - kvp.Value;
        //         float distSqr = diff.sqrMagnitude;

        //         if (distSqr < minDistanceSqrB && nearestA != kvp.Value)
        //         {
        //             minDistanceSqrB = distSqr;
        //             nearestB = kvp.Value;

        //             // THIS LINE IS IMPORTANT
        //             _grabbableSO.IndexB = kvp.Key;
        //         }
        //     }

        //     foreach (KeyValuePair<int, Vector3> kvp in _watchedVertices)
        //     {
        //         Vector3 diff = pos - kvp.Value;
        //         float distSqr = diff.sqrMagnitude;

        //         if (distSqr < minDistanceSqrC && nearestA != kvp.Value && nearestB != kvp.Value)
        //         {
        //             minDistanceSqrC = distSqr;
        //             nearestC = kvp.Value;

        //             // THIS LINE IS IMPORTANT
        //             _grabbableSO.IndexC = kvp.Key;
        //         }
        //     }

        //     Vector3 nearest = (nearestA + nearestB + nearestC) / 3;
        //     return nearest;
        // }




                // // Check if the nearest vertex has changed.
                // int[] nearest = FindNearestTo(_handStateSO.PrimaryHand.Position);
                // if (!CheckIfIdentical(_grabbableSO.PrevNearest, nearest, 1))
                // {
                //     // Debug.Log("New nearest is at: [" + string.Join(", ", nearest) + "]");

                //     string type = "none";
                //     Vector3 position = Vector3.zero;

                //     if (nearest[0] != -1 && nearest[1] == -1 && nearest[2] == -1)
                //     {
                //         type = "vertex";
                //         position = _planetStateSO.Octree.Get(nearest[0]);
                //     }
                //     else if (nearest[0] != -1 && nearest[1] != -1 && nearest[2] == -1)
                //     {
                //         type = "edge";
                //         position = GetMidPoint(_planetStateSO.Octree.Get(0), _planetStateSO.Octree.Get(1));
                //     }
                //     else if (nearest[0] != -1 && nearest[1] != -1 && nearest[2] != -1)
                //     {
                //         type = "triangle";
                //         position = GetMidPoint(_planetStateSO.Octree.Get(0), _planetStateSO.Octree.Get(1), _planetStateSO.Octree.Get(2));
                //     }
                //     else
                //     {
                //         return;
                //     }

                //     Debug.Log("Nearest " + type + " is at position " + position.ToString());

                //     _prefabManager.ClearChildren();
                //     _prefabManager.AddGrabbable(type, position);
                //     _grabbableSO.PrevNearest = nearest;
                //     // return;
                // }
            // }





















        // private int[] FindNearestTo(Vector3 position)
        // {
            // // convert position to local space
            // position = transform.InverseTransformPoint(position);

            // // scan all vertices to find nearest
            // int nearestVertex = -1;
            // float minDistanceSqr = Mathf.Infinity;
            // List<int> verticesInOctant = _planetStateSO.Octree.GetAll();

            // for (int i = 0; i < verticesInOctant.Count; i++)
            // {
            //     Vector3 vertex = _planetStateSO.Octree.Get(verticesInOctant[i]);
            //     Vector3 diff = position - vertex;
            //     float distSqr = diff.sqrMagnitude;

            //     if (distSqr < minDistanceSqr)
            //     {
            //         minDistanceSqr = distSqr;
            //         nearestVertex = i;
            //     }
            // }
            // // Debug.Log("Nearest Vertex: " + nearestVertex);

            // // convert nearest vertex back to world space
            // return new int[3] { nearestVertex, -1, -1 };
        //     return new int[3] { -1, -1, -1 };
        // }

        // private bool CheckIfIdentical(int[] a, int[] b, int numVertices)
        // {
        //     // // Debug.Log("a: [" + string.Join(", ", a) + "]");
        //     // // Debug.Log("b: [" + string.Join(", ", b) + "]");
        //     // if (a.Length != b.Length) return false;
        //     // for (int i = 0; i < numVertices; i++)
        //     // {
        //     //     if (a[i] != b[i]) return false;
        //     // }
        //     return true;
        // }

        // private void AddGrabbable(Vector3 position, int[] vertices)
        // {
        //     _debugger.Log("Adding grabbable from [" + string.Join(", ", vertices) + "]");
        //     if (vertices.Length < 3) return;

        //     if (vertices[0] != -1 && vertices[1] == -1 && vertices[2] == -1)
        //     {
        //         // _debugger.Log("Hmm");
        //         Debug.Log(_debugCounter++ + "");
        //         if (_grabbableContainer.childCount == 0)
        //         {
        //             GameObject signifier = Instantiate(_vertexPrefab, _grabbableContainer);
        //         }
        //         return;
        //     }

        //     // GameObject signifier;

        //     // if (vertices[0] == -1)
        //     // {
        //     //     _debugger.Log("Invalid vertex.");
        //     //     return;
        //     // }
        //     // else if (vertices[1] == -1)
        //     // {
        //     //     _debugger.Log("...of type vertex.");
        //     //     // Instantiate(_vertexPrefab, _grabbableContainer);

        //     //     // signifier = Instantiate(_vertexPrefab, transform);
        //     //     // signifier.transform.position = _gameSO.State.PlanetState.Octree.Get(vertices[0]);

        //     //     _debugger.Log("...at position " + transform.position);
        //     //     // _debugger.Log("...at position " + signifier.transform.position.ToString());
        //     //     // signifier.transform.position = _gameSO.State.PlanetState.Vertices[vertices[0]];
        //     // }
        //     // else if (vertices[2] == -1)
        //     // {
        //     //     _debugger.Log("...of type edge.");
        //     //     // signifier = Instantiate(_edgePrefab, transform);
        //     //     // signifier.transform.position = GetMidPoint(vertices[0], vertices[1]);
        //     // }
        //     // else if (vertices[2] != -1)
        //     // {
        //     //     _debugger.Log("...of type triangle.");
        //     //     // signifier = Instantiate(_trianglePrefab, transform);
        //     //     // signifier.transform.position = GetMidPoint(vertices[0], vertices[1], vertices[2]);
        //     // }
        //     // else
        //     // {
        //     //     return;
        //     // }

        //     // _debugger.Log("Yay!");

        //     // _gameSO.CurrentAxisConstraint = signifier.transform.position.normalized;
        //     // _gameSO.CurrentVertexIds = vertices;

        // }












        

        // private int[] FindNearest()
        // {
        //     HandState primaryHand = _handStateSO.PrimaryHand;
        //     HandState altHand = _handStateSO.AltHand;

        //     // if (Vector3.Distance(_lastHandPosition, primaryHand.Position) < 0.01f) return _prevNearest;
        //     // _lastHandPosition = primaryHand.Position;

        //     if (altHand.ActivePose == "VertexSelector")
        //     {
        //         return NearestVertexTo(primaryHand.Position);
        //     }
        //     // else if (altHand.ActivePose == "EdgeSelector")
        //     // {
        //     //     return NearestEdgeTo(primaryHand.Position);
        //     // }
        //     // else if (altHand.ActivePose == "TriangleSelector")
        //     // {
        //     //     return NearestTriangleTo(primaryHand.Position);
        //     // }
        //     else
        //     {
        //         // _debugger.Log("Invalid pose for geometry grab interaction.");
        //         return new int[3] { -1, -1, -1 };
        //     }
        // }

        // private int[] NearestVertexTo(Vector3 position)
        // {
        //     // convert position to local space
        //     position = transform.InverseTransformPoint(position);

        //     // scan all vertices to find nearest
        //     int nearestVertex = -1;
        //     float minDistanceSqr = Mathf.Infinity;
        //     List<int> verticesInOctant = GetVerticesInCurrentOctant();

        //     // Debug.Log(verticesInOctant.Count + " vertices in selected octants.");

        //     // Debug.Log(verticesInOctant.Count + "");

        //     for (int i = 0; i < verticesInOctant.Count; i++)
        //     {
        //         Vector3 vertex = _planetStateSO.Octree.Get(verticesInOctant[i]);
        //         Vector3 diff = position - vertex;
        //         float distSqr = diff.sqrMagnitude;

        //         if (distSqr < minDistanceSqr)
        //         {
        //             minDistanceSqr = distSqr;
        //             nearestVertex = i;
        //         }
        //     }

        //     // convert nearest vertex back to world space
        //     return new int[3] { nearestVertex, -1, -1 };
        // }

        // /// TODO: Potential bug; the two vertices may not be connected.
        // private int[] NearestEdgeTo(Vector3 position)
        // {
        //     // convert position to local space
        //     position = transform.InverseTransformPoint(position);

        //     // scan all vertices to find nearest
        //     int nearestVertexA = -1;
        //     float minDistanceSqr = Mathf.Infinity;
        //     List<int> verticesInOctant = GetVerticesInCurrentOctant();

        //     for (int i = 0; i < verticesInOctant.Count; i++)
        //     {
        //         Vector3 vertex = _planetStateSO.Octree.Get(verticesInOctant[i]);
        //         Vector3 diff = position - vertex;
        //         float distSqr = diff.sqrMagnitude;

        //         if (distSqr < minDistanceSqr)
        //         {
        //             minDistanceSqr = distSqr;
        //             nearestVertexA = i;
        //         }
        //     }

        //     // scan all vertices to find second nearest
        //     int nearestVertexB = -1;
        //     minDistanceSqr = Mathf.Infinity;

        //     for (int j = 0; j < verticesInOctant.Count; j++)
        //     {
        //         if (j == nearestVertexA) continue;
        //         Vector3 vertex = _planetStateSO.Octree.Get(verticesInOctant[j]);
        //         Vector3 diff = position - vertex;
        //         float distSqr = diff.sqrMagnitude;

        //         if (distSqr < minDistanceSqr)
        //         {
        //             minDistanceSqr = distSqr;
        //             nearestVertexB = j;
        //         }
        //     }

        //     // convert nearest vertex back to world space
        //     return new int[3] { nearestVertexA, nearestVertexB, -1 };
        // }

        // // private int[] NearestTriangleTo(Vector3 position)
        // // {
        // //     // convert position to local space
        // //     position = transform.InverseTransformPoint(position);

        // //     // scan all triangles to find nearest
        // //     MeshTriangle nearestTriangle = new MeshTriangle(-1, -1, -1);
        // //     float minDistanceSqr = Mathf.Infinity;
        // //     List<int> trianglesInOctant = GetTrianglesInCurrentOctant();

        // //     for (int i = 0; i < trianglesInOctant.Count; i++)
        // //     {
        // //         MeshTriangle poly = _planetStateSO.MeshTriangles[trianglesInOctant[i]];
        // //         if (!poly.IsSide)
        // //         {
        // //             Vector3 center = _planetStateSO.GetTriangleMidPoint(i);
        // //             Vector3 diff = position - center;
        // //             float distSqr = diff.sqrMagnitude;

        // //             if (distSqr < minDistanceSqr)
        // //             {
        // //                 minDistanceSqr = distSqr;
        // //                 nearestTriangle = poly;
        // //             }
        // //         }
        // //     }
            
        // //     return nearestTriangle.VertexIndices.ToArray();
        // // }



        // // octant1 = +x +y +z
        // // octant2 = -x +y +z
        // // octant3 = -x +y -z
        // // octant4 = +x +y -z
        // // octant5 = +x -y +z
        // // octant6 = -x -y +z
        // // octant7 = -x -y -z
        // // octant8 = +x -y -z
        // private List<int> GetVerticesInCurrentOctant()
        // {
        //     List<int> vertices = new List<int>();

        //     // foreach (int octant in _curOctants)
        //     // {
        //     //     // _debugger.Log("There are " + octantVertices.Count + " vertices in octant #" + octant);
        //     //     vertices.AddRange(_planetStateSO.Octree.GetVerticesInOctant(octant));
        //     // }

        //     // vertices = vertices.Distinct().ToList();

        //     return vertices;
        // }

        // private List<int> GetTrianglesInCurrentOctant()
        // {
        //     PlanetState planet = _gameSO.State.PlanetState;
        //     List<int> triangles = new List<int>();

        //     foreach (int octant in _curOctants)
        //     {
        //         triangles.AddRange(planet.GetTrianglesInOctant(octant));
        //     }

        //     triangles = triangles.Distinct().ToList();

        //     return triangles;
        // }



        /// <summary>
        ///   Returns false when the two arrays are not equal.
        /// </summary>
        // private bool CompareNearest(int[] candidate)
        // {
        //     // // _debugger.Log("Comparing nearest.");
        //     // if (candidate.Length != _prevNearest.Length) return false;
        //     // for (int i = 0; i < candidate.Length; i++)
        //     // {
        //     //     if (candidate[i] != _prevNearest[i]) return false;
        //     // }
        //     return true;
        // }


        // private void ClearChildren()
        // {
        //     _debugger.Log("Clearing children.");
        //     while (_grabbableContainer.childCount > 0)
        //     {
        //         Destroy(_grabbableContainer.GetChild(0).gameObject);
        //     }
        // }
    }
}