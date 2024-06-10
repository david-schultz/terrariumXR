using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;
using TerrariumXR.EventSystem;

namespace TerrariumXR.Geometry
{
    [CreateAssetMenu(fileName="SteppedSlider_Data", menuName="UI/Stepped Slider")]
    public class SteppedSliderSO : ScriptableObject
    {
        [Tooltip("Inclusive")]
        public int MajorSteps = 9;

        [Tooltip("Exclusive")]
        public int Divisions  = 4;

        [Tooltip("Inclusive")]
        public int Steps { get { return ((MajorSteps - 1) * (Divisions)) + 1; } }

        public int CurStep = 3;
        public bool ShouldStepSmall = true;
        
        // Tracks each triangle's index, and maps it to a height
        // public Dictionary<int, float> triangleHeights; 
    }
}