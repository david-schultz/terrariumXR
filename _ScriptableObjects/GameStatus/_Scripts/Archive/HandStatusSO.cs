using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR.EventSystem;


//TODO: Why not save grabbablevertexSO in here?
namespace TerrariumXR
{
    [CreateAssetMenu(fileName="HandStatus_Data", menuName="Global/Hand Status")]
    public class HandStatusSO : ScriptableObject
    {
        public string Tag = "n/a"; // e.g. "LeftHand"
        public Vector3 Position = new Vector3(0, 0, 0);
        public Vector3 FingertipPosition = new Vector3(0, 0, 0);
        public bool IsPrimary = false;
        public string ActivePose = "n/a";

        public string Status = "inactive"; // e.g. "selecting"
        // public string CurrentPose = "neutral"; // e.g. "single finger point"
        public GrabbableVertexSO GrabbableVertex;

        public Dictionary<string, bool> Poses = new Dictionary<string, bool>();
    }
}