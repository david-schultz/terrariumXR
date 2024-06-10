using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;

namespace TerrariumXR.EventSystem
{
    public class GrabCommand : ICommand
    {
        public string Type { get; }
        public GameState State { get; }
        
        public GrabCommand(GameState newState)
        {
            Type = "grab";
            State = newState.Clone();
        }
    }
}