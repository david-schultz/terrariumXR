using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StellarBody
{
    public class OrbitalRotation : MonoBehaviour
    {
        [SerializeField]
        private float _rotationSpeed;

        // [SerializeField]
        private Vector3 _localAxis = Vector3.up;
        private Transform _target;


        #region Properties
        public Transform Target
        {
            get => _target;
            set => _target = value;
        }

        public float RotationSpeed
        {
            get => _rotationSpeed;
            set => _rotationSpeed = value;
        }

        public Vector3 LocalAxis
        {
            get => _localAxis;
            set => _localAxis = value;
        }

        #endregion

        protected virtual void Update()
        {
            transform.RotateAround(_target.position, _localAxis, _rotationSpeed * Time.deltaTime);
            transform.Rotate(_localAxis, _rotationSpeed * 2 * Time.deltaTime, Space.Self);
        }
    }
}

