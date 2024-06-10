using System;
using UnityEngine;
using Oculus.Interaction;
using TerrariumXR.EventSystem;
using TerrariumXR.Geometry;

namespace TerrariumXR
{
    public class SteppedOneGrabTransformer : MonoBehaviour, ITransformer
    {
    // ================== Event Broadcaster ==================
        [SerializeField] private IntIntIntEventChannelSO _stepUpdateChannel;

    // ================== Transform Variables ==================
        private IGrabbable _grabbable;
        private Vector3 _grabOffsetInLocalSpace; // The offset between the grab point and the object's center
        private float _xStepSize;
        private float _yStepSize;
        private float _zStepSize;

        private int _xCurStep = 0;
        private int _yCurStep = 0;
        private int _zCurStep = 0;

    // ================== Constraint Variables ==================
        [SerializeField] private float _xMin = 0f;
        [SerializeField] private float _xMax = 0f;
        [SerializeField] private float _yMin = 0f;
        [SerializeField] private float _yMax = 0.15f;
        [SerializeField] private float _zMin = 0f;
        [SerializeField] private float _zMax = 0f;

        [SerializeField] private int _xSteps = 1;
        [SerializeField] private int _ySteps = 2;
        [SerializeField] private int _zSteps = 1;

    // ================== Functions ==================
        public void Initialize(IGrabbable grabbable)
        {
            _grabbable = grabbable;
            if (_xSteps > 1) _xStepSize = _xMax / (_xSteps - 1);
            if (_ySteps > 1) _yStepSize = _yMax / (_ySteps - 1);
            if (_zSteps > 1) _zStepSize = _zMax / (_zSteps - 1);

            // /// Set position to CurStep
            // Transform targetTransform = _grabbable.Transform;
            // Vector3 pos = new Vector3(0, 0, _sliderSO.CurStep * _stepSize);


            // // Convert the constrained position back to world space
            // if (targetTransform.parent != null)
            // {
            //     pos = targetTransform.parent.TransformPoint(pos);
            // }

            // targetTransform.position = pos;
        }

        public void BeginTransform()
        {
            Pose grabPoint = _grabbable.GrabPoints[0];
            Transform targetTransform = _grabbable.Transform;
            _grabOffsetInLocalSpace = targetTransform.InverseTransformVector(grabPoint.position - targetTransform.position);
        }

        /// Here, we need to constrain translation to max/min X values.
        /// Additionally, movement needs to snap to either big or small steps, depending on
        /// whether small steps are enabled.
        public void UpdateTransform()
        {
            Pose grabPoint = _grabbable.GrabPoints[0];
            Transform targetTransform = _grabbable.Transform;
            Vector3 heldPosition = grabPoint.position - targetTransform.TransformVector(_grabOffsetInLocalSpace);

            // Convert to parent space
            if (targetTransform.parent != null)
            {
                heldPosition = targetTransform.parent.InverseTransformPoint(heldPosition);
            }

            // Apply constraints
            heldPosition.x = Mathf.Max(heldPosition.x, _xMin);
            heldPosition.x = Mathf.Min(heldPosition.x, _xMax);
            heldPosition.y = Mathf.Max(heldPosition.y, _yMin);
            heldPosition.y = Mathf.Min(heldPosition.y, _yMax);
            heldPosition.z = Mathf.Max(heldPosition.z, _zMin);
            heldPosition.z = Mathf.Min(heldPosition.z, _zMax);

            int xNearest = NearestStepTo("x", heldPosition.x);
            int yNearest = NearestStepTo("y", heldPosition.y);
            int zNearest = NearestStepTo("z", heldPosition.z);

            Vector3 constrainedPosition = new Vector3(xNearest * _xStepSize, yNearest * _yStepSize, zNearest * _zStepSize);

            // Convert the constrained position back to world space
            if (targetTransform.parent != null)
            {
                constrainedPosition = targetTransform.parent.TransformPoint(constrainedPosition);
            }

            // Update position
            targetTransform.position = constrainedPosition;

            // Broadcast event
            if (_xCurStep != xNearest || _yCurStep != yNearest || _zCurStep != zNearest)
            {
                _xCurStep = xNearest;
                _yCurStep = yNearest;
                _zCurStep = zNearest;
                _stepUpdateChannel?.RaiseEvent(xNearest, yNearest, zNearest);
            }
        }

        public void EndTransform()
        {
            // // TODO: Save state through command
            // ICommand command = new SaveCommand(_gameSO.State);
            // _commandChannel?.RaiseEvent(command);
        }



        /// If ShouldStepSmall, return small step amt
        /// Otherwise, return big step amt
        public int NearestStepTo(string axis, float value)
        {
            if (axis == "x") return (int) Mathf.Round(value / _xStepSize);
            if (axis == "y") return (int) Mathf.Round(value / _yStepSize);
            if (axis == "z") return (int) Mathf.Round(value / _zStepSize);
            return -1;
        }

        public void InjectCurrentStep(int x, int y, int z)
        {
            if (x != -1) _xCurStep = x;
            if (y != -1) _yCurStep = y;
            if (z != -1) _zCurStep = z;

            Transform targetTransform = _grabbable.Transform;
            
            Vector3 constrainedPosition = new Vector3(_xCurStep * _xStepSize, _yCurStep * _yStepSize, _zCurStep * _zStepSize);
            // Convert the constrained position back to world space
            if (targetTransform.parent != null)
            {
                constrainedPosition = targetTransform.parent.TransformPoint(constrainedPosition);
            }

            // Update position
            targetTransform.position = constrainedPosition;
        }
    }
}