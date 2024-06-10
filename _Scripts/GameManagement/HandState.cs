using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.Interaction
{
    public class HandState
    {
        public string Tag = "n/a";
        public bool IsPrimary = false;
        public string ActivePose = "n/a";
        public Vector3 Position = Vector3.zero;
        // public Vector3 FingertipPosition = Vector3.zero;

        public HandState(string tag)
        {
            Tag = tag;
        }

        public HandState(string tag, bool isPrimary)
        {
            Tag = tag;
            IsPrimary = isPrimary;
        }
    }
}