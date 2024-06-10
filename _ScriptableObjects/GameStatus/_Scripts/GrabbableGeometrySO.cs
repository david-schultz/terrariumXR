using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR
{
    [CreateAssetMenu(fileName="GrabbableGeometryData", menuName="Global/GrabbableGeometryData")]
    public class GrabbableGeometrySO : ScriptableObject
    {
        public bool IsGrabbed = false;
        public Vector3 Direction = Vector3.zero;
        public float CurrentDistance = 0.25f;
        public float StandardDistance = 0.25f;
        public float MinDistance = 0.23f;
        public float MaxDistance = 0.31f;
        // Used to track which vertex is being referenced
        // When -1, indicates that no vertex is being referenced
        public int[] VertIds = new int[3] { -1, -1, -1 };
        public float[] InitialVertDist = new float[3] { 0.25f, 0.25f, 0.25f };
        public float[] VertDist = new float[3] { 0.25f, 0.25f, 0.25f };

        public Vector3 PrevNearest = Vector3.zero;

        public void Initialize()
        {
            Initialize(0.25f, 0.23f, 0.31f);
        }

        public void Initialize(float standardDistance, float minDistance, float maxDistance)
        {
            IsGrabbed = false;
            Direction = Vector3.zero;
            CurrentDistance = standardDistance;
            StandardDistance = standardDistance;
            MinDistance = minDistance;
            MaxDistance = maxDistance;

            VertIds = new int[3] { -1, -1, -1 };
            InitialVertDist = new float[3] { standardDistance, standardDistance, standardDistance };
            VertDist = new float[3] { InitialVertDist[0], InitialVertDist[1], InitialVertDist[2] };

            PrevNearest = Vector3.zero;
        }

        public void Reset()
        {
            IsGrabbed = false;
            Direction = Vector3.zero;
            CurrentDistance = StandardDistance;

            VertIds[0] = -1;
            VertIds[1] = -1;
            VertIds[2] = -1;

            InitialVertDist[0] = StandardDistance;
            InitialVertDist[1] = StandardDistance;
            InitialVertDist[2] = StandardDistance;

            VertDist[0] = InitialVertDist[0];
            VertDist[1] = InitialVertDist[1];
            VertDist[2] = InitialVertDist[2];

            PrevNearest = Vector3.zero;
        }

        public void Set()
        {
            IsGrabbed = false;
            InitialVertDist[0] = VertDist[0];
            InitialVertDist[1] = VertDist[1];
            InitialVertDist[2] = VertDist[2];
        }
    }
}