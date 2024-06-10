using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;
using TerrariumXR.EventSystem;

namespace TerrariumXR.Geometry
{
    [CreateAssetMenu(fileName="TriangleHeights_Data", menuName="Global/Triangle Heights")]
    public class TriangleHeightsSO : ScriptableObject
    {
        // Tracks each triangle's index, and maps it to a height
        public Dictionary<int, float> triangleHeights; 
    }
}