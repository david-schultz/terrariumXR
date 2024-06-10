using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;

namespace TerrariumXR.EventSystem
{
    /// <summary>
    /// ICommand objects are used to apply an IGameState.
    /// </summary>
    public interface ICommand
    {
        string Type {get;}
        GameState State {get;}
    }
}
