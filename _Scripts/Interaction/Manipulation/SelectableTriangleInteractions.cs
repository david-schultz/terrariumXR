// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Oculus.Interaction;
// using Oculus.Interaction.Surfaces;
// using TerrariumXR.EventSystem;
// using TerrariumXR.Geometry;
// using TerrariumXR.UI;

// namespace TerrariumXR
// {
//     /// <summary>
//     /// Primary files involved:
//     ///  - HandStatusSO.cs (via GameStatusSO.cs)
//     ///  - HandPositionBroadcaster.cs
//     ///  - MeshCollisionBroadcaster.cs
//     ///  - PoseCommandBroadcaster.cs
//     /// </summary>
//     /// 
//     /// TODO:
//     ///  - Do NOT select extruded triangles
//     public class SelectableTriangleInteractions : MonoBehaviour
//     {
//         [SerializeField] private Debugger _debugger;
//         [SerializeField] private GameStatusSO _gameSO;
//         [SerializeField] private CommandEventChannelSO _commandChannel;
//         [SerializeField] private StringStringEventChannelSO _meshCollisionChannel; //false: left, true: right
//         [SerializeField] private PlanetStateEventChannelSO _meshUpdateChannel; //TODO: refactor for efficiency, use triangleSelectChannel instead of meshUpdateChannel.
//         [SerializeField] private StringEventChannelSO _gamemodeChannel;
//         [SerializeField] private IntEventChannelSO _triangleSelectionChannel; 

//         [SerializeField] private GameObject _extrudor;

//     // ================== Event Management ==================
//         private void OnEnable()
//         {
//             _meshCollisionChannel.OnEventRaised += UpdateCollision;
//             _gamemodeChannel.OnEventRaised += ClearSelection;
//         }
//         private void OnDisable()
//         {
//             _meshCollisionChannel.OnEventRaised -= UpdateCollision;
//             _gamemodeChannel.OnEventRaised -= ClearSelection;
//         }
//         private void UpdateCollision(string hand, string state)
//         {
//             // if (_gameSO.LeftHandStatus.Status == "exited" && _gameSO.LeftHandStatus.Status == "exited")
//             // {
//             //     _debugger.Log("both exited");
//             //     // ShowExtrudor();
                
//             //     // ICommand command = new SelectTriangleCommand(_gameSO.State.Clone());
//             //     // _commandChannel?.RaiseEvent(command);

//             //     return; // Do not run select any triangles after saving state
//             // }

//             if (hand == "left")
//             {
//                 UpdateSelection(_gameSO.LeftHandStatus);
//             }
//             else if (hand == "right")
//             {
//                 UpdateSelection(_gameSO.RightHandStatus);
//             }
//         }

//         private void UpdateSelection(HandStatusSO hand)
//         {
//             // _debugger.Log(hand.Tag + " is " + hand.State);
//             // Hand exited collider: Send command to save state (for undo/redo)
//             if (hand.Status == "exited")
//             {
//                 _debugger.Log("exited");
//                 // _debugger.Log("Selecting " + _gameSO.State.SelectedTriangles.Count + " triangles...");
//                 // ICommand command = new SelectTriangleCommand(_gameSO.State.Clone());
//                 // _commandChannel?.RaiseEvent(command);

//                 // Do not run select any triangles after saving state
//                 return;
//             }
//             else 
//             {
//                 int nearest = NearestTriangleTo(hand.FingertipPosition);
//                 if (hand.Poses["single finger point"])
//                 {
//                     if (!_gameSO.State.SelectedTriangles.Contains(nearest))
//                     {
//                         if (!_gameSO.State.PlanetState.MeshTriangles[nearest].IsSide)
//                         {
//                             _gameSO.State.SelectedTriangles.Add(nearest);
//                             MeshTriangle t = _gameSO.State.PlanetState.MeshTriangles[nearest];
//                             t.IsSelected = true;
//                         }
//                     }

//                 }
//                 else if (hand.Poses["double finger point"])
//                 {
//                     if (_gameSO.State.SelectedTriangles.Contains(nearest))
//                     {
//                         _gameSO.State.SelectedTriangles.Remove(nearest);
//                         MeshTriangle t = _gameSO.State.PlanetState.MeshTriangles[nearest];
//                         t.IsSelected = false;
//                     }
//                 }

//                 if (_gameSO.State.SelectedTriangles.Count > 0)
//                 {
//                     _extrudor.SetActive(true);
//                 }
//                 else
//                 {
//                     _extrudor.SetActive(false);
//                 }

//                 FillSelection();
//                 _triangleSelectionChannel?.RaiseEvent(nearest);
//                 // _meshUpdateChannel?.RaiseEvent(_gameSO.State.PlanetState);
//             }
            
//         }




//     // ================== Helpers ==================

//         // TODO: rotate given point around the origin
//         public int NearestTriangleTo(Vector3 point)
//         {
//             // convert point to local space
//             point = transform.InverseTransformPoint(point);

//             // scan all mesh triangles to find nearest
//             int nearestTriangle = -1;
//             float minDistanceSqr = Mathf.Infinity;

//             for (int i = 0; i < _gameSO.State.PlanetState.MeshTriangles.Count; i++)
//             {
//                 if (!_gameSO.State.PlanetState.MeshTriangles[i].IsSide)
//                 {
//                     // compute the center of the triangle
//                     Vector3 center = _gameSO.State.PlanetState.GetTriangleMidPoint(i);

//                     Vector3 diff = point - center;
//                     float distSqr = diff.sqrMagnitude;

//                     if (distSqr < minDistanceSqr)
//                     {
//                         minDistanceSqr = distSqr;
//                         nearestTriangle = i;
//                     }
//                 }
//             }

//             // convert nearest vertex back to world space
//             return nearestTriangle;
//         }

//         private void FillSelection()
//         {

//         }

//         private void ClearSelection(string gamemode)
//         {
//             if (gamemode != "view")
//             {
//                 // Go through each triangle, and set is selected to false
//                 foreach (int id in _gameSO.State.SelectedTriangles)
//                 {
//                     MeshTriangle t = _gameSO.State.PlanetState.MeshTriangles[id];
//                     t.IsSelected = false;
//                 }

//                 // Reset SelectedTriangles
//                 _gameSO.State = new GameState(_gameSO.State.PlanetState.Clone());

//                 // Update mesh
//                 _meshUpdateChannel.RaiseEvent(_gameSO.State.PlanetState);

//                 _extrudor.SetActive(false);
//             }
//         }
//     }
// }