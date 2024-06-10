using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;

namespace TerrariumXR.EventSystem
{
    public class UpdateDictionaryCommand : ICommand
    {
        public string Type { get; }
        public GameState State { get; }
        
        public UpdateDictionaryCommand(GameState newState)
        {
            Type = "update-dictionary";
            State = newState;
        }
    }
}