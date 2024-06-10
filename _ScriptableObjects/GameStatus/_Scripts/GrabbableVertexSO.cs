using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: What if i saved this data in GameStatusSO?
namespace TerrariumXR
{
    [CreateAssetMenu(fileName="GrabbableVertex", menuName="Interaction/Grabbable Vertex")]
    public class GrabbableVertexSO : ScriptableObject
    {
        // Used to track which vertex is being referenced
        // When -1, indicates that no vertex is being referenced
        public int Index = -1;

        public Vector3 Direction;
        public Vector3 Position;
        public float MinDistance = 0.23f;
        public float MaxDistance = 0.31f;

        public bool IsGrabbed = false;
        public int PrevNearest = -1;
    }
}
