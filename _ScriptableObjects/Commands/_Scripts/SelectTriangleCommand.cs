using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;

namespace TerrariumXR.EventSystem
{
    public class SelectTriangleCommand : ICommand
    {
        public string Type { get; }
        public GameState State { get; }
        
        public SelectTriangleCommand(GameState newState)
        {
            Type = "select-triangle";
            State = newState.Clone();
        }
    }
}