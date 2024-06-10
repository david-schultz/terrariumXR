using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR.EventSystem;

namespace TerrariumXR.Interaction
{
    public class GrabbableTypeManager : MonoBehaviour
    {
    // ================== References ==================
        [SerializeField] private Debugger _debugger;
        [SerializeField] private PlanetStateSO _planetStateSO;
        [SerializeField] private HandStateSO _handStateSO;
        [SerializeField] private GrabbableGeometrySO _grabbableSO;

        [SerializeField] private GameObject _grabbableObject;
        [SerializeField] private Mesh _vertexMesh;
        [SerializeField] private Mesh _edgeMesh;
        [SerializeField] private Mesh _triangleMesh;
        // [SerializeField] private Color32 _vertexColor;
        // [SerializeField] private Color32 _edgeColor;
        // [SerializeField] private Color32 _triangleColor;

    // ================== Variables ==================
        // private List<Vector3>[] _vertices;

    // ================== Event Channels ==================
        [SerializeField] private BoolStringEventChannelSO _handPoseEventChannel;
        [SerializeField] private VoidEventChannelSO _grabbableMovedEventChannel;

        private void OnEnable()
        {
            _handPoseEventChannel.OnEventRaised += PoseDetected;
            _grabbableMovedEventChannel.OnEventRaised += AdjustScale;
        }

        private void OnDisable()
        {
            _handPoseEventChannel.OnEventRaised -= PoseDetected;
            _grabbableMovedEventChannel.OnEventRaised += AdjustScale;
        }

        private void PoseDetected(bool isLeftPrimary, string activePose)
        {
            if (activePose == "VertexSelector")
            {
                _grabbableObject.GetComponent<MeshFilter>().mesh = _vertexMesh;
            }
            else if (activePose == "EdgeSelector")
            {
                _grabbableObject.GetComponent<MeshFilter>().mesh = _edgeMesh;
            }
            else if (activePose == "TriangleSelector")
            {
                _grabbableObject.GetComponent<MeshFilter>().mesh = _triangleMesh;
            }

            AdjustScale();
        }


        private void AdjustScale()
        {
            if (_handStateSO.AltHand.ActivePose == "VertexSelector")
            {
                _grabbableObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            }
            else if (_handStateSO.AltHand.ActivePose == "EdgeSelector")
            {
                _grabbableObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                // 1. get proper x length
                // distance between point a and point b
                // Vector3 v1 = _planetStateSO.Octree.Get(_grabbableSO.VertIds[0]);
                // Vector3 v2 = _planetStateSO.Octree.Get(_grabbableSO.VertIds[1]);
                // _grabbableObject.transform.localScale = new Vector3(Vector3.Distance(v1, v2), 0.005f, 0.005f);

                // 2. rotate to align with edge
                // _grabbableObject.transform.rotation = Quaternion.LookRotation(_grabbableSO.Direction, Vector3.up);
            }
            else if (_handStateSO.AltHand.ActivePose == "TriangleSelector")
            {
                _grabbableObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            }
        }

    }
}