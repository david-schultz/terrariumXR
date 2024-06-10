using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;

namespace TerrariumXR.EventSystem
{
    /// <summary>
    /// CommandListener listens to _commandChannel, executing + tracking them as they come.
    /// It tracks a stack of GameStates, which enables undo/redo functionality.
    /// </summary>
    ///
    /// <todo> Save memory by storing a max # of undo commands. Use a deque w/ max length. </todo>
    public class CommandListener : MonoBehaviour
    {
        [SerializeField] private Debugger _debugger;
        [SerializeField] private GameStatusSO _gameSO;

        [SerializeField] private CommandEventChannelSO _commandChannel;
        [SerializeField] private StringEventChannelSO _gamemodeChannel;
        [SerializeField] private PlanetStateEventChannelSO _meshUpdateChannel;
        // [SerializeField] private VoidEventChannelSO _extrudeChannel;

        private GameState _baseState;
        [SerializeField] private BoolEventChannelSO _toggleMenuChannel;
        private Stack<GameState> _undoStack;
        private Stack<GameState> _redoStack;

    // ================== Initialization ==================
        void Start()
        {
            _undoStack = new Stack<GameState>();
            _redoStack = new Stack<GameState>();
        }

        private void OnEnable()
        {
            _gamemodeChannel.OnEventRaised += UpdateGamemode;
            _commandChannel.OnEventRaised += HandleCommand;
        }

        private void OnDisable()
        {
            _gamemodeChannel.OnEventRaised -= UpdateGamemode;
            _commandChannel.OnEventRaised -= HandleCommand;
        }

    // ================== Handle received commands ==================
        private void UpdateGamemode(string gamemode)
        {
            _gameSO.Gamemode = gamemode;
            _debugger.Log(" • changed to " + gamemode + " mode.");
        }

        private void HandleCommand(ICommand command)
        {
            _debugger.Log("Received " + command.Type + " command...");

            if (command.Type == "initialize")
            {
                _gameSO.InitialState = command.State;
                _baseState = command.State;
                ApplyState(command.State);
            }
            else if (command.Type == "undo")
            {
                UndoCommand(); 
            }
            else if (command.Type == "redo")
            {
                RedoCommand(); 
            }
            else if (command.Type == "extrude")
            {
                // _extrudeChannel?.RaiseEvent();
                ApplyState(command.State); 
                // this should mainly be used to save state
            }
            else
            {
                // _undoStack.Push(command.State);
                // _redoStack.Clear();
                // ApplyState(command.State); 
            }

            _debugger.Log("U=" + _undoStack.Count + " R=" + _redoStack.Count);
            _debugger.Log("");
        }

        private void UndoCommand()
        {
            if (_undoStack.Count > 0)
            {
                GameState state = _undoStack.Pop();
                _redoStack.Push(state);
                
                if (_undoStack.Count > 1)
                {
                    ApplyState(_undoStack.Peek());
                }
                else
                {
                    // ApplyState(_initialState);
                    ApplyState(_baseState);
                }
            }
        }

        private void RedoCommand()
        {
            // _debugger.Log("RedoStack count: " + _redoStack.Count);
            if (_redoStack.Count > 0)
            {
                GameState state = _redoStack.Pop();
                _undoStack.Push(state);
                ApplyState(state);
            }
        }

    // ================== Command execution ==================
        private void ApplyState(GameState state)
        {
            bool shouldUpdatePlanetState = state.PlanetState != null;
            bool shouldUpdateSelectedVertices = state.SelectedVertices != null;
            bool shouldUpdateSelectedTriangles = state.SelectedTriangles != null; 

            // _debugger.Log("Received state:");
            // _debugger.Log("——planetstate:         " + shouldUpdatePlanetState);
            // _debugger.Log("——selectedvertices:  " + shouldUpdateSelectedVertices);
            // _debugger.Log("——selectedtriangles: " + shouldUpdateSelectedTriangles);
            // _debugger.Log("");

            
            Debug.Log("Saving state with " + state.PlanetState.Octree.Count + " vertices.");
            _gameSO.State = state;
            _meshUpdateChannel?.RaiseEvent(state.PlanetState);

            // if (shouldUpdatePlanetState)
            // {
            //     _meshUpdateChannel.RaiseEvent(state.PlanetState);
            // }

            if (shouldUpdateSelectedTriangles)
            {
                // foreach (int t in state.SelectedTriangles)
                // {

                // }
                // _meshUpdateChannel.RaiseEvent(state.PlanetState);
                // _highlightTriangleGroupChannel.RaiseEvent();
            }
        }
    }
}