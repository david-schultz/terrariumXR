using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using TerrariumXR.EventSystem;

namespace TerrariumXR
{
    public class SphericalConstraintOneGrabRotateTransformer : MonoBehaviour, ITransformer
    {
        [SerializeField] private float _radius;
        [SerializeField] private Vector3EventChannelSO _lookAtChannel;

    // ================== Transformer Vars ==================
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
        }

        public void UpdateTransform()
        {
            // The original method (OneGrabTranslateTransformer) works by first
            // getting the "held" position of the grab point (constrainedPosition),
            // then adjusting its x, y, z values according to constraints.

            // Here, we again start with the "held" position, but then need to constrain
            // it along the edge of the sphere.

            Pose grabPoint = _grabbable.GrabPoints[0];
            Transform targetTransform = _grabbable.Transform;
            // Vector3 direction = targetTransform.TransformVector(_grabOffsetInLocalSpace).normalized;
            Vector3 heldPosition = grabPoint.position - targetTransform.TransformVector(_grabOffsetInLocalSpace);

            // Convert to parent space
            if (targetTransform.parent != null)
            {
                heldPosition = targetTransform.parent.InverseTransformPoint(heldPosition);
            }

            Vector3 direction = heldPosition.normalized;

            // float dotProduct = Vector3.Dot(heldPosition, direction);
            // if (dotProduct < min) dotProduct = min;
            // if (dotProduct > max) dotProduct = max;
            // Vector3 constrainedPosition = dotProduct * direction;
            
            Vector3 constrainedPosition = _radius * direction;

            // float dotProduct = Vector3.Dot(heldPosition, direction);
            // if (dotProduct < min) dotProduct = min;
            // if (dotProduct > max) dotProduct = max;
            // Vector3 constrainedPosition = dotProduct * direction;
            // _grabbableVertexSO.Position = constrainedPosition;




            // Convert the constrained position back to world space
            if (targetTransform.parent != null)
            {
                constrainedPosition = targetTransform.parent.TransformPoint(constrainedPosition);
            }

            targetTransform.position = constrainedPosition;

            _lookAtChannel?.RaiseEvent(direction);
        }

        public void EndTransform()
        {

        }
    }
}