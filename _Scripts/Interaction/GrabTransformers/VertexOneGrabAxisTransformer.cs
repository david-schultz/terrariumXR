
using System;
using UnityEngine;
using Oculus.Interaction;
using TerrariumXR.EventSystem;

namespace TerrariumXR
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


    public class VertexOneGrabAxisTransformer : MonoBehaviour, ITransformer
    {
    // ================== Event Broadcasting ==================
        // [SerializeField] private VertexEventBroadcaster _broadcaster;
        // [SerializeField] private BoolEventChannelSO _vertexGrabbedChannel;
        // [SerializeField] private Vector3EventChannelSO _vertexPositionChannel;
        // [SerializeField] private VoidEventChannelSO _vertexUpdateChannel;
        [SerializeField] private VoidEventChannelSO _vertexReleasedChannel;

    // ================== Transform Variables ==================
        [SerializeField] private GrabbableVertexSO _grabbableVertexSO; //make sure this is set to the proper left/right side
        [SerializeField] private GrabbableVertexSO _grabbableVertexRight;

        private IGrabbable _grabbable;
        private Vector3 _initialPosition;
        private Vector3 _grabOffsetInLocalSpace; // The offset between the grab point and the object's center

    // ================== Functions ==================
        public void Initialize(IGrabbable grabbable)
        {
            _grabbable = grabbable;
            _initialPosition = _grabbable.Transform.localPosition;
        }

        public void BeginTransform()
        {
            var grabPoint = _grabbable.GrabPoints[0];
            Transform targetTransform = _grabbable.Transform;
            _grabOffsetInLocalSpace = targetTransform.InverseTransformVector(grabPoint.position - targetTransform.position);
            
            _grabbableVertexSO.IsGrabbed = true;
            // Broadcast event
            // _broadcaster.VertexGrabbed();
        }

        public void UpdateTransform()
        {
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
            Vector3 direction = _grabbableVertexSO.Direction;
            float min = _grabbableVertexSO.MinDistance;
            float max = _grabbableVertexSO.MaxDistance;

            // Convert to parent space
            if (targetTransform.parent != null)
            {
                heldPosition = targetTransform.parent.InverseTransformPoint(heldPosition);
                // direction = targetTransform.parent.InverseTransformPoint(direction);
            }

            // TODO:
            // • Add friction near the end of the max distance
            
            float dotProduct = Vector3.Dot(heldPosition, direction);
            if (dotProduct < min) dotProduct = min;
            if (dotProduct > max) dotProduct = max;
            Vector3 constrainedPosition = dotProduct * direction;
            _grabbableVertexSO.Position = constrainedPosition;

            // Broadcast event
            // note: constrainedPosition must be in local space
            // _broadcaster.VertexMoved(constrainedPosition);
            // _vertexPositionChannel.RaiseEvent(constrainedPosition);

            // Convert the constrained position back to world space
            if (targetTransform.parent != null)
            {
                constrainedPosition = targetTransform.parent.TransformPoint(constrainedPosition);
            }

            targetTransform.position = constrainedPosition;
        }

        public void EndTransform()
        {
            _grabbableVertexSO.IsGrabbed = false;
            _vertexReleasedChannel.RaiseEvent();

            // Broadcast event
            // _broadcaster.VertexReleased();
        }

        public void SetAxis(Vector3 axis)
        {
            _grabbableVertexSO.Direction = axis;
        }
    }
}