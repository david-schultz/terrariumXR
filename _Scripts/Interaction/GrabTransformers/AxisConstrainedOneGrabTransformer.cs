
using System;
using UnityEngine;
using Oculus.Interaction;
using TerrariumXR.EventSystem;

namespace TerrariumXR.Interaction
{

    /// <summary>
    /// This class does two things:
    /// 1. Constrains translation along a given axis.
    /// 2. Fires events when the object is grabbed, moved, or released.
    /// Grab events are watched by MeshManager, to determine when a vertex is being manipulated.
    /// </summary>
    /// 
    /// <remarks>
    /// This class is a modified version of the OneGrabTranslateTransformer class from the Oculus SDK.
    /// Retrieved from https://communityforums.atmeta.com/t5/Unity-VR-Development/Check-if-an-object-is-grabbed/td-p/959742
    /// </remarks>
    /// 
    /// <todo>
    ///   • It would be awesome to add "friction" near the end of the max distance.
    /// </todo>


    public class AxisConstrainedOneGrabTransformer : MonoBehaviour, ITransformer
    {
    // ================== Event Broadcasting ==================
        [SerializeField] private Debugger _debugger;
        [SerializeField] private GrabbableGeometrySO _grabbableSO;
        [SerializeField] private VoidEventChannelSO _grabbableMovedEventChannel;
        // [SerializeField] private GrabbablePositionEventChannelSO _grabbablePositionEventChannel;

    // ================== Transform Variables ==================
        private float _minDistance;
        private float _maxDistance;
        // [SerializeField] private float _minDistance;
        // [SerializeField] private float _maxDistance;
        private IGrabbable _grabbable;
        private Vector3 _initialPosition;
        private Vector3 _grabOffsetInLocalSpace; // The offset between the grab point and the object's center
        // private Vector3 _axisConstraint;

        // private int[] _vertexIds = {-1};

        // private bool _axisInjected = false;
        // private bool _idsInjected = false;

    // ================== Functions ==================
        public void Initialize(IGrabbable grabbable)
        {
            _grabbable = grabbable;
            _initialPosition = _grabbable.Transform.localPosition;
            _minDistance = _grabbableSO.MinDistance;
            _maxDistance = _grabbableSO.MaxDistance;
            // Debug.Log("Initialized.");
        }

        public void BeginTransform()
        {
            // if (!_axisInjected || !_idsInjected) return;
            // Debug.Log("Beginning transform.");

            Pose grabPoint = _grabbable.GrabPoints[0];
            Transform targetTransform = _grabbable.Transform;
            _grabOffsetInLocalSpace = targetTransform.InverseTransformVector(grabPoint.position - targetTransform.position);

            _grabbableSO.IsGrabbed = true;
        }

        public void UpdateTransform()
        {

            // Debug.Log("Updating transform.");
            // if (!_axisInjected || !_idsInjected) return;
            // The original method (OneGrabTranslateTransformer) works by first
            // getting the "held" position of the grab point (constrainedPosition),
            // then adjusting its x, y, z values according to constraints.

            // Here, we again start with the "held" position, but then use it to find
            // the dot product between it + a constraint vector. We then apply
            // that dot product on a projection of the constraint vector.
            // Finally, before setting the new position, we constrain it to a max/min value.

            Pose grabPoint = _grabbable.GrabPoints[0];
            Transform targetTransform = _grabbable.Transform;
            Vector3 heldPosition = grabPoint.position - targetTransform.TransformVector(_grabOffsetInLocalSpace);

            // Convert to parent space
            if (targetTransform.parent != null)
            {
                heldPosition = targetTransform.parent.InverseTransformPoint(heldPosition);
            }
            
            float dotProduct = Vector3.Dot(heldPosition, _grabbableSO.Direction);
            if (dotProduct < _minDistance) dotProduct = _minDistance;
            if (dotProduct > _maxDistance) dotProduct = _maxDistance;
            Vector3 constrainedPosition = dotProduct * _grabbableSO.Direction;

            _grabbableSO.CurrentDistance = dotProduct;
            // UpdateOctree();

                        // float dotProduct = Vector3.Dot(heldPosition, _gameSO.CurrentAxisConstraint);
                        // if (dotProduct < _minDistance) dotProduct = _minDistance;
                        // if (dotProduct > _maxDistance) dotProduct = _maxDistance;
                        // Vector3 constrainedPosition = dotProduct * _gameSO.CurrentAxisConstraint;

                        // float dotProduct = Vector3.Dot(heldPosition, _axisConstraint);
                        // if (dotProduct < _minDistance) dotProduct = _minDistance;
                        // if (dotProduct > _maxDistance) dotProduct = _maxDistance;
                        // Vector3 constrainedPosition = dotProduct * _axisConstraint;

            // Broadcast event
            // note: constrainedPosition must be in local space
            // _broadcaster.VertexMoved(constrainedPosition);
            // _vertexPositionChannel.RaiseEvent(constrainedPosition);
                    // GrabbablePositionData data = new GrabbablePositionData(_gameSO.CurrentVertexIds, dotProduct);
                    // _grabbablePositionEventChannel?.RaiseEvent(data);
            _grabbableMovedEventChannel?.RaiseEvent();

            // Convert the constrained position back to world space
            if (targetTransform.parent != null)
            {
                constrainedPosition = targetTransform.parent.TransformPoint(constrainedPosition);
            }

            targetTransform.position = constrainedPosition;
        }

        public void EndTransform()
        {
            // _grabbableVertexSO.IsGrabbed = false;

            // Broadcast event
            // _vertexReleasedChannel?.RaiseEvent();
            // _broadcaster.VertexReleased();
            // UpdateOctree();
            _grabbableMovedEventChannel?.RaiseEvent();
            _grabbableSO.Set();
        }


        // private void UpdateOctree()
        // {
        //     if (_grabbableSO.IndexA != -1)
        //     {
        //         _planetStateSO.Octree.Adjust(_grabbableSO.IndexA, _grabbableSO.Direction * _grabbableSO.CurrentDistance);
        //     }            
        // }




        // #region Inject

        // public void InjectAxisConstraint(Vector3 axis)
        // {
        //     _axisConstraint = axis.normalized;
        //     _axisInjected = true;
        // }

        // public void InjectIds(int[] vertexIds)
        // {
        //     _vertexIds = vertexIds;
        //     _idsInjected = true;
        // }

        // #endregion
    }
}