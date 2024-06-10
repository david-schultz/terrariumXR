// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace TerrariumXR.EventSystem
// {
//     public class PoseCommandBroadcaster : MonoBehaviour
//     {
//         [SerializeField] private Debugger _debugger;
//         [SerializeField] private CommandEventChannelSO _commandChannel;
//         [SerializeField] private StringEventChannelSO _gamemodeChannel;
//         [SerializeField] private GameStatusSO _gameStatusSO;

//         [SerializeField] private VoidEventChannelSO _feelingLuckyChannel;

//         public void TriggerBunny()
//         {
//             // ICommand command = new ExtrudeCommand(_gameStatusSO.State);
//             // _commandChannel?.RaiseEvent(command);
//             // _feelingLuckyChannel?.RaiseEvent();
//         }
        
//         /// <summary>
//         /// SingleFingerGun is used to trigger Undo commands.
//         /// </summary>
//         public void TriggerSingleFingerGun()
//         {
//             ICommand command = new UndoCommand();
//             _commandChannel?.RaiseEvent(command);
//         }

//         /// <summary>
//         /// SingleFingerGun is used to trigger Redo commands.
//         /// </summary>
//         public void TriggerDoubleFingerGun()
//         {
//             ICommand command = new RedoCommand();
//             _commandChannel?.RaiseEvent(command);
//         }

//         /// <summary>
//         /// This sequence is used to change gamemode to geometry mode.
//         /// </summary>
//         public void TriggerTurnThumbToUpGesture()
//         {
//             // ICommand command = new GamemodeCommand("geometry");
//             // _commandChannel.RaiseEvent(command);
//             _gamemodeChannel?.RaiseEvent("geometry");
//         }

//         /// <summary>
//         /// This sequence is used to change gamemode to view mode.
//         /// </summary>
//         public void TriggerTurnThumbToSideGesture()
//         {
//             // ICommand command = new GamemodeCommand("view");
//             // _commandChannel.RaiseEvent(command);
//             _gamemodeChannel?.RaiseEvent("view");
//         }

//         [Tooltip("Input format: Tag|Shape|State")]
//         public void UpdateHandShape(string input)
//         {
//             string tag = input.Split('|')[0];
//             string shape = input.Split('|')[1];
//             bool state = bool.Parse(input.Split('|')[2]);

//             HandStatusSO hand = tag == "LeftHand" ? _gameStatusSO.LeftHandStatus : _gameStatusSO.RightHandStatus;
//             hand.Poses[shape] = state;

//             // _debugger.Log(hand.Poses[shape] + "");
//             // ICommand command = new PoseShapeCommand("view");
//             // _commandChannel.RaiseEvent(command);
//         }

//         public void SwipeUpGesture()
//         {
//             _debugger.Log("detected swipe up");
//             ICommand command = new SwipeUpCommand();
//             _commandChannel.RaiseEvent(command);
//         }
//     }
// }