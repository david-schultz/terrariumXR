// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TerrariumXR;
// using TerrariumXR.Geometry;

// namespace TerrariumXR.EventSystem
// {
//     public class GrabbableVertexInteractions : MonoBehaviour
//     {
//         [SerializeField] private Debugger _debugger;
//         [SerializeField] private GameStatusSO _gameSO;

//         [SerializeField] private CommandEventChannelSO _commandChannel;
//         [SerializeField] private PlanetStateEventChannelSO _meshUpdateChannel;
//         [SerializeField] private StringEventChannelSO _gamemodeChannel;

//         [SerializeField] private VoidEventChannelSO _leftVertexReleasedChannel;
//         [SerializeField] private VoidEventChannelSO _rightVertexReleasedChannel;
//         [SerializeField] private VertexPositionEventChannelSO _vertexPositionChannel;

//         [SerializeField] private Transform _parent;
//         [SerializeField] private GameObject _leftGrabbableVertexPrefab;
//         [SerializeField] private GameObject _rightGrabbableVertexPrefab;
//         [SerializeField] private Transform _grabbableVertexContainer;

//         private static Vector3 origin = new Vector3(0, 0, 0);

//     // ================== Event Management ==================
//         private void OnEnable()
//         {
//             _leftVertexReleasedChannel.OnEventRaised += LeftVertexReleased;
//             _rightVertexReleasedChannel.OnEventRaised += RightVertexReleased;
//             _gamemodeChannel.OnEventRaised += RemoveAllVertices;
//         }

//         private void OnDisable()
//         {
//             _leftVertexReleasedChannel.OnEventRaised -= LeftVertexReleased;
//             _rightVertexReleasedChannel.OnEventRaised -= RightVertexReleased;
//             _gamemodeChannel.OnEventRaised -= RemoveAllVertices;
//         }

//         private void LeftVertexReleased()
//         {
//             ReleaseVertex(_gameSO.LeftHandStatus);
//         }

//         private void RightVertexReleased()
//         {
//             ReleaseVertex(_gameSO.RightHandStatus);
//         }

        
//     // ================== State Management ==================
//         void Update()
//         {
//             if (_gameSO.Gamemode != "geometry") return;

//             UpdateVertices(_gameSO.LeftHandStatus);
//             UpdateVertices(_gameSO.RightHandStatus);
//         }

//         private void UpdateVertices(HandStatusSO hand)
//         {
//             GrabbableVertexSO grabbable = hand.GrabbableVertex;
//             // _debugger.Log(hand.Tag + " / " + grabbable.IsGrabbed);
//             if (!grabbable.IsGrabbed)
//             {
//                 // Check if the hand has the right pose
//                 // if (hand.Poses != null && !hand.Poses["grab"])
//                 // {
//                 //     RemoveVertex(hand);
//                 //     return;
//                 // }

//                 // Check if the nearest vertex has changed.
//                 int nearest = NearestVertexTo(hand.Position);
//                 if (grabbable.PrevNearest != nearest)
//                 {
//                     RemoveVertex(hand);
//                     AddVertex(hand, nearest);
//                     grabbable.PrevNearest = nearest;
//                 }
//             }
//             else
//             {
//                 MoveVertex(grabbable.Index, grabbable.Position);
//             }
//         }

//         public void MoveVertex(int index, Vector3 newPosition)
//         {
//             _gameSO.State.PlanetState.Vertices[index] = newPosition;
//             _meshUpdateChannel.RaiseEvent(_gameSO.State.PlanetState);
//             // _vertexPositionChannel?.RaiseEvent(index, newPosition);
//         }

//         private void ReleaseVertex(HandStatusSO hand)
//         {
//             GrabbableVertexSO grabbable = hand.GrabbableVertex;
//             ICommand command = new UpdateDictionaryCommand(_gameSO.State);
//             _commandChannel.RaiseEvent(command);

//             // Reset nearest vector, clear grabbable object
//             grabbable.Index = -1;
//             grabbable.PrevNearest = -1;
//             RemoveVertex(hand);
//         }

//     // ================== Helpers ==================
//         public GameObject AddVertex(HandStatusSO hand, int vertexIndex)
//         {
//             GrabbableVertexSO grabbable = hand.GrabbableVertex;
//             Vector3 vertex = _gameSO.State.PlanetState.Vertices[vertexIndex];

//             // update scriptable object
//             grabbable.Index = vertexIndex;
//             grabbable.Position = vertex;
//             grabbable.Direction = vertex.normalized;

//             // add new grabbable point
//             GameObject prefab = hand.Tag == "LeftHand" ? _leftGrabbableVertexPrefab : _rightGrabbableVertexPrefab;
//             GameObject vertexObject = Instantiate(prefab, _grabbableVertexContainer);
//             vertexObject.transform.Translate(vertex * _parent.localScale.x);

//             return vertexObject;
//         }

//         public void RemoveVertex(HandStatusSO hand)
//         {
//             string handSide = "";
//             if (hand.Tag == "LeftHand") handSide = "left";
//             if (hand.Tag == "RightHand") handSide = "right";
//             foreach (Transform child in _grabbableVertexContainer)
//             {
//                 string childSide = "";
//                 if (child.tag == "LeftVertex") childSide = "left";
//                 if (child.tag == "RightVertex") childSide = "right";
//                 if (childSide == handSide)
//                 {
//                     Destroy(child.gameObject);
//                 }
//             }
//         }

//         public int NearestVertexTo(Vector3 point)
//         {
//             // convert point to local space
//             point = transform.InverseTransformPoint(point);

//             // scan all vertices to find nearest
//             int nearestVertex = -1;
//             float minDistanceSqr = Mathf.Infinity;
            
//             // for (int i = 0; i < _gameSO.InitialState.PlanetState.Vertices.Count; i++)
//             // {
//             //     Vector3 vertex = _gameSO.InitialState.PlanetState.Vertices[i];
//             //     Vector3 diff = point - vertex;
//             //     float distSqr = diff.sqrMagnitude;

//             //     if (distSqr < minDistanceSqr)
//             //     {
//             //         minDistanceSqr = distSqr;
//             //         nearestVertex = i;
//             //     }
//             // }

//             for (int i = 0; i < _gameSO.State.PlanetState.MeshTriangles.Count; i++)
//             {
//                 MeshTriangle poly = _gameSO.State.PlanetState.MeshTriangles[i];
//                 if (!poly.IsSide)
//                 {
//                     foreach (int j in poly.VertexIndices)
//                     {
//                         Vector3 vertex = _gameSO.State.PlanetState.Vertices[j];
//                         Vector3 diff = point - vertex;
//                         float distSqr = diff.sqrMagnitude;

//                         if (distSqr < minDistanceSqr)
//                         {
//                             minDistanceSqr = distSqr;
//                             nearestVertex = j;
//                         }
//                     }
//                 }
//             }

//             // convert nearest vertex back to world space
//             return nearestVertex;
//         }

//         // public int NearestVertexTo(Vector3 point)
//         // {
//         //     // convert point to local space
//         //     point = transform.InverseTransformPoint(point);

//         //     // scan all vertices to find nearest
//         //     int nearestVertex = -1;
//         //     float minDistanceSqr = Mathf.Infinity;

//         //     for (int i = 0; i < _gameSO.ActivePlanet.Vertices.Count; i++)
//         //     {
//         //         Vector3 vertex = _gameSO.ActivePlanet.Vertices[i];
//         //         Vector3 diff = point - vertex;
//         //         float distSqr = diff.sqrMagnitude;

//         //         if (distSqr < minDistanceSqr)
//         //         {
//         //             minDistanceSqr = distSqr;
//         //             nearestVertex = i;
//         //         }
//         //     }

//         //     // convert nearest vertex back to world space
//         //     return nearestVertex;
//         // }

//         // private void HandPosition(Vector3 position)
//         // {
//         //     if (_gameSO.ActiveGamemode != "geometry") return;
//         //     if (_gameSO.IsHandColliding && !_grabbableVertexSO.IsGrabbed)
//         //     {
//         //         // Check if the nearest vertex has changed.
//         //         int nearest = NearestVertexTo(position);
//         //         if (_grabbableVertexSO.PrevNearest != nearest)
//         //         {
//         //             // _debugger.Log("Adding grabbable vertex...");
//         //             RemoveGrabbableVertices();
//         //             AddGrabbableVertex(nearest);
//         //             _grabbableVertexSO.PrevNearest = nearest;
//         //         }
//         //     }
//         //     else if (!_gameSO.IsHandColliding && !_grabbableVertexSO.IsGrabbed)
//         //     {
//         //         // Reset nearest vector
//         //         _grabbableVertexSO.PrevNearest = -1;
//         //         // Clear all vertices
//         //         RemoveGrabbableVertices();
//         //     }
//         // }

//     // ================== Interactions ==================
//         // Receives a change in VertexGrab state.
//         // private void VertexGrabbed(bool isGrabbed) {
//         //     _grabbableVertexSO.IsGrabbed = isGrabbed;
//         //     if (isGrabbed) { 
                
//         //     }
//         //     else { 
//         //         VertexReleased();
//         //     }
//         // }

//         // private void VertexMoved(Vector3 position)
//         // {
//         //     // _debugger.Log("Vertex moved: " + position);
//         //     // MoveVertex(_grabbableVertexSO.Index, position);

//         //     // 1. Create a copy of the active planet state.
//         //     // 2. Update the copy's vertex position.
//         //     // 3. Pass the copy through the mesh update channel.
//         //     PlanetState clonedState = _gameSO.ActivePlanet.Clone();
//         //     clonedState.Vertices[_grabbableVertexSO.Index] = position;
//         //     _meshUpdateChannel.RaiseEvent(clonedState);
//         // }



//     // // ================== Vertex Interactions ==================
//         // public void RemoveVertex(HandStatusSO hand)
//         // {
//         //     foreach (Transform child in _grabbableVertexContainer)
//         //     {
//         //         if (child.tag == hand.tag)
//         //         {
//         //             Destroy(child.gameObject);
//         //         }
//         //     }
//         // }

//         // public GameObject AddVertex(HandStatusSO hand, int vertexIndex)
//         // {
//         //     GrabbableVertexSO grabbable = hand.GrabbableVertex;
//         //     Vector3 vertex = _gameSO.ActivePlanet.Vertices[vertexIndex];

//         //     // update scriptable object
//         //     grabbable.Index = vertexIndex;
//         //     grabbable.Position = vertex;
//         //     grabbable.Direction = vertex.normalized;

//         //     // add new grabbable point
//         //     GameObject vertexObject = Instantiate(_grabbableVertexPrefab, _grabbableVertexContainer);
//         //     vertexObject.transform.Translate(vertex);

//         //     return vertexObject;
//         // }

//         public void RemoveAllVertices(string gamemode)
//         {
//             RemoveVertex(_gameSO.LeftHandStatus);
//             RemoveVertex(_gameSO.RightHandStatus);
//             // foreach (Transform child in _grabbableVertexContainer) {
//             //     Destroy(child.gameObject);
//             // }

//             // _grabbableVertexSO.Index = -1;
//             // _grabbableVertexSO.Direction = Vector3.zero;
//             // _grabbableVertexSO.Position = Vector3.zero;
//         }

//         // public void AddAllVertices()
//         // {
//         //     RemoveGrabbableVertices();

//         //     // Potential bug: because there is only one GrabbableVertexSO,
//         //     // only one vertex will have proper data behind it.
//         //     for (int i = 0; i < _gameSO.ActivePlanet.Vertices.Count; i++)
//         //     {
//         //         AddGrabbableVertex(i);
//         //     }
//         // }

//     }
// }