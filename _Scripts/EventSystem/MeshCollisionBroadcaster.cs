// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TerrariumXR;

// namespace TerrariumXR.EventSystem
// {
//     public class MeshCollisionBroadcaster : MonoBehaviour
//     {
//         [SerializeField] private Debugger _debugger;
//         [SerializeField] private GameStatusSO _gameSO;
//         [SerializeField] private StringStringEventChannelSO _meshCollisionChannel;

//         private HandStatusSO _leftHandSO;
//         private HandStatusSO _rightHandSO;

//         void Start()
//         {
//             _leftHandSO = _gameSO.LeftHandStatus;
//             _rightHandSO = _gameSO.RightHandStatus;
//         }
        
//     // ================== Interactivity ==================
//         void OnTriggerEnter(Collider other)
//         {
//             if (_gameSO.Gamemode != "view") return;

//             // filter out non-hand objects
//             if (!other.CompareTag("LeftTipPointer") && !other.CompareTag("RightTipPointer")) {
//                 return;
//             }

//             // _debugger.Log("entered: " + other.gameObject.tag);
//             if (other.CompareTag("LeftTipPointer"))
//             {
//                 // _debugger.Log(other.gameObject.tag + "entered.");
//                 _leftHandSO.Status = "entered";
//                 _meshCollisionChannel.RaiseEvent("left", "entered");
//             }

//             if (other.CompareTag("RightTipPointer"))
//             {
//                 // _debugger.Log(other.gameObject.tag + "entered.");
//                 _rightHandSO.Status = "entered";
//                 _meshCollisionChannel.RaiseEvent("right", "entered");
//             }
//         }

//         void OnTriggerStay(Collider other)
//         {
//             if (_gameSO.Gamemode != "view") return;

//             // filter out non-hand objects
//             if (!other.CompareTag("LeftTipPointer") && !other.CompareTag("RightTipPointer")) {
//                 return;
//             }

//             // _debugger.Log("stay: " + other.gameObject.tag);
//             if (other.CompareTag("LeftTipPointer"))
//             {
//                 _leftHandSO.Status = "colliding";
//                 _meshCollisionChannel.RaiseEvent("left", "colliding");
//             }

//             if (other.CompareTag("RightTipPointer"))
//             {
//                 _rightHandSO.Status = "colliding";
//                 _meshCollisionChannel.RaiseEvent("right", "colliding");
//             }
//         }

//         void OnTriggerExit(Collider other)
//         {
//             if (_gameSO.Gamemode != "view") return;

//             // filter out non-hand objects
//             if (!other.CompareTag("LeftTipPointer") && !other.CompareTag("RightTipPointer")) {
//                 return;
//             }

//             _debugger.Log("exited: " + other.gameObject.tag);
//             if (other.CompareTag("LeftTipPointer"))
//             {
//                 _leftHandSO.Status = "exited";
//                 _meshCollisionChannel.RaiseEvent("left", "exited");
//             }

//             if (other.CompareTag("RightTipPointer"))
//             {
//                 _rightHandSO.Status = "exited";
//                 _meshCollisionChannel.RaiseEvent("right", "exited");
//             }
//         }

//     }
// }