using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;

namespace TerrariumXR.EventSystem
{
    public class SwipeUpCommand : ICommand
    {
        public string Type { get; }
        public GameState State { get; }
        
        public SwipeUpCommand()
        {
            Type = "swipe-up";
            // toggle swipe menu
            State.Menus = new Dictionary<string, bool>();
            State.Menus.Add("swipe-menu", true);
        }
    }
}