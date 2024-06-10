using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;

namespace TerrariumXR.EventSystem
{
    public class SaveCommand : ICommand
    {
        public string Type { get; }
        public GameState State { get; }
        
        public SaveCommand(GameState state)
        {
            Type = "save";
            State = state.Clone();
        }
    }
}