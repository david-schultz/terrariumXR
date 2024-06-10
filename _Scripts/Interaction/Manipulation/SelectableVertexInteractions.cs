// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Oculus.Interaction;
// using Oculus.Interaction.Surfaces;
// using TerrariumXR.EventSystem;
// using TerrariumXR.Geometry;

// namespace TerrariumXR
// {
//     /// <summary>
//     /// Primary files involved:
//     ///  - HandStatusSO.cs (via GameStatusSO.cs)
//     ///  - HandPositionBroadcaster.cs
//     ///  - MeshCollisionBroadcaster.cs
//     ///  - PoseCommandBroadcaster.cs
//     /// </summary>
//     public class SelectableVertexInteractions : MonoBehaviour
//     {
//         [SerializeField] private Debugger _debugger;
//         [SerializeField] private GameStatusSO _gameStatusSO;
//         [SerializeField] private CommandEventChannelSO _commandChannel;
//         [SerializeField] private BoolEventChannelSO _meshCollisionChannel; //false: left, true: right

//         [SerializeField] private GameObject _selectedVertexPrefab;
//         [SerializeField] private Transform _container;
//         [SerializeField] private Transform _parent;

//     // ================== Event Management ==================
//         private void OnEnable()
//         {
//             _meshCollisionChannel.OnEventRaised += UpdateCollision;
//         }
//         private void OnDisable()
//         {
//             _meshCollisionChannel.OnEventRaised -= UpdateCollision;
//         }
//         private void UpdateCollision(bool hand)
//         {
//             if (!hand)
//             {
//                 UpdateSelection(_gameStatusSO.LeftHandStatus);
//             }
//             else
//             {
//                 UpdateSelection(_gameStatusSO.RightHandStatus);
//             }
//         }

//         public void UpdateSelection(HandStatusSO hand)
//         {
//             if (hand.Poses["single finger point"])
//             {
//                 int nearest = NearestVertexTo(hand.FingertipPosition);
                
//                 if (!_gameStatusSO.State.SelectedVertices.Contains(nearest))
//                 {
//                     _gameStatusSO.State.SelectedVertices.Add(nearest);
//                     AddVertex(nearest);
                    
//                     // _debugger.Log(hand.Tag + "|single|" + hand.State + "|" + nearest);
//                 }
//             }
//             else if (hand.Poses["double finger point"])
//             {
//                 int nearest = NearestVertexTo(hand.FingertipPosition);
//                 if (_gameStatusSO.State.SelectedVertices.Contains(nearest))
//                 {
//                     _gameStatusSO.State.SelectedVertices.Remove(nearest);
//                     RemoveVertex(nearest);
                    
//                     // _debugger.Log(hand.Tag + "|double|" + hand.State + "|" + nearest);
//                 }
//             }

//             // get nearest vertex
//             // add vertex to tempSelection
//         }

//         public void OnRelease()
//         {
//             // save tempSelection to new gamestate
//         }

//         /// <summary>
//         /// 
//         /// </summary>
//         /// <param name="vertex"></param>
//         public void AddSelection(List<int> vertex)
//         {

//         }


//     // ================== Helpers ==================
//         public void AddVertex(int vertexIndex)
//         {
//             Vector3 vertex = _gameStatusSO.State.PlanetState.Vertices[vertexIndex];

//             // add new grabbable point
//             GameObject vertexObject = Instantiate(_selectedVertexPrefab, _container);
//             vertexObject.name = "vertex" + vertexIndex;
//             vertexObject.transform.Translate(vertex * _parent.localScale.x);
//         }

//         public void RemoveVertex(int vertexIndex)
//         {
//             foreach (Transform child in _container)
//             {
//                 if (child.name == "vertex" + vertexIndex)
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

//             for (int i = 0; i < _gameStatusSO.State.PlanetState.Vertices.Count; i++)
//             {
//                 Vector3 vertex = _gameStatusSO.State.PlanetState.Vertices[i];
//                 Vector3 diff = point - vertex;
//                 float distSqr = diff.sqrMagnitude;

//                 if (distSqr < minDistanceSqr)
//                 {
//                     minDistanceSqr = distSqr;
//                     nearestVertex = i;
//                 }
//             }

//             // convert nearest vertex back to world space
//             return nearestVertex;
//         }
//     }
// }