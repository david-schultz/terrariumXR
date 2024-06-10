
using System;
using UnityEngine;
using Oculus.Interaction;
using TerrariumXR.EventSystem;
using TerrariumXR.Geometry;

namespace TerrariumXR
{
    public class SteppedSliderOneGrabTransformer : MonoBehaviour, ITransformer
    {
    // ================== Scriptable Objects ==================
        [SerializeField] private GameStatusSO _gameSO;
        [SerializeField] private SteppedSliderSO _sliderSO;

    // ================== Event Broadcasting ==================
        [SerializeField] private Debugger _debugger;
        [SerializeField] private CommandEventChannelSO _commandChannel;
        [SerializeField] private VoidEventChannelSO _sliderUpdateChannel;

    // ================== Transform Variables ==================
        private IGrabbable _grabbable;
        private Vector3 _grabOffsetInLocalSpace; // The offset between the grab point and the object's center

    // ================== Constraint Variables ==================
        [SerializeField] private float _minZ = 0f;
        [SerializeField] private float _maxZ = 0.15f;

        private float _stepSize;
        private float _majorStepSize;

        // private float _minHeight = 0.23f;
        // private float _maxHeight = 0.31f;

    // ================== Functions ==================
        public void Initialize(IGrabbable grabbable)
        {
            _grabbable = grabbable;
            _stepSize = _maxZ / (_sliderSO.Steps - 1);
            _majorStepSize = _maxZ / (_sliderSO.MajorSteps - 1);

            /// Set position to CurStep
            Transform targetTransform = _grabbable.Transform;
            Vector3 pos = new Vector3(0, 0, _sliderSO.CurStep * _stepSize);


            // Convert the constrained position back to world space
            if (targetTransform.parent != null)
            {
                pos = targetTransform.parent.TransformPoint(pos);
            }

            targetTransform.position = pos;
        }

        public void BeginTransform()
        {
            Pose grabPoint = _grabbable.GrabPoints[0];
            Transform targetTransform = _grabbable.Transform;
            _grabOffsetInLocalSpace = targetTransform.InverseTransformVector(grabPoint.position - targetTransform.position);
            
            //TODO: create and stitch extruded triangles?
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

            heldPosition.x = 0;
            heldPosition.y = 0;
            heldPosition.z = Mathf.Max(heldPosition.z, _minZ);
            heldPosition.z = Mathf.Min(heldPosition.z, _maxZ);

            int nearest = NearestStepTo(heldPosition.z);
            Vector3 constrainedPosition = new Vector3(0, 0, nearest * _stepSize);
            
            // Broadcast event
            if (_sliderSO.CurStep != nearest)
            {
                // _debugger.Log(nearest + "");
                // _debugger.Log(constrainedPosition.ToString());
                _sliderSO.CurStep = nearest;
                _sliderUpdateChannel?.RaiseEvent();
            }

            // Convert the constrained position back to world space
            if (targetTransform.parent != null)
            {
                constrainedPosition = targetTransform.parent.TransformPoint(constrainedPosition);
            }

            targetTransform.position = constrainedPosition;
        }

        public void EndTransform()
        {
            // TODO: Save state through command
            ICommand command = new SaveCommand(_gameSO.State);
            _commandChannel?.RaiseEvent(command);
        }



        /// If ShouldStepSmall, return small step amt
        /// Otherwise, return big step amt
        public int NearestStepTo(float z)
        {
            if (_sliderSO.ShouldStepSmall)
            {
                return (int) Mathf.Round(z / _stepSize);
            }

            return (int) Mathf.Round(z / _majorStepSize) * _sliderSO.Divisions;
        }
    }
}