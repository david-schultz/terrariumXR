using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR.EventSystem;
using TerrariumXR.Geometry;
using TerrariumXR.Interaction;


//TODO: Why not save grabbablevertexSO in here?
namespace TerrariumXR
{
    [CreateAssetMenu(fileName="GameStatus_Data", menuName="Global/Game Status")]
    public class GameStatusSO : ScriptableObject
    {
        public string Gamemode = "geometry";
        public GameState InitialState;
        public GameState State;

        public Vector3 CurrentAxisConstraint;
        public int[] CurrentVertexIds = {-1};
    }
}