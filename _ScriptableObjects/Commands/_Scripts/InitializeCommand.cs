using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.EventSystem
{
    public class InitializeCommand : ICommand
    {
        public string Type { get; }
        public GameState State { get; }
        
        public InitializeCommand(GameState initialState)
        {
            Type = "initialize";
            State = initialState.Clone();
        }
    }
}