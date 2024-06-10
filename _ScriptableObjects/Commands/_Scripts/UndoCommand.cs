using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;

namespace TerrariumXR.EventSystem
{
    public class UndoCommand : ICommand
    {
        public string Type { get; }
        public GameState State { get; }
        
        public UndoCommand()
        {
            Type = "undo";
            // State can be null, as it is already tracked in CommandListener
        }
    }
}