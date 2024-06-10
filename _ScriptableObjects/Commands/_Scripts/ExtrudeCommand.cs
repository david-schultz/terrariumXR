using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;

namespace TerrariumXR.EventSystem
{
    public class ExtrudeCommand : ICommand
    {
        public string Type { get; }
        public GameState State { get; }
        
        public ExtrudeCommand(GameState state)
        {
            Type = "extrude";
            State = state.Clone();
        }
    }
}