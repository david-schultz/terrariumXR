using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TerrariumXR;

namespace TerrariumXR.EventSystem
{
    public class SelectVertexCommand : ICommand
    {
        public string Type { get; }
        public GameState State { get; }
        
        public SelectVertexCommand(GameState newState)
        {
            Type = "select-vertex";
            State = newState.Clone();
        }
    }
}