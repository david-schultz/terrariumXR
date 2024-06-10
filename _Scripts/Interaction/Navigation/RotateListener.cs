using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

namespace TerrariumXR.EventSystem
{
    public class SphericalRotate : MonoBehaviour
    {
        [SerializeField] private Vector3EventChannelSO _lookAtChannel;

        private void Start()
        {
            LookAt(Vector3.forward);
        }

        private void OnEnable()
        {
            _lookAtChannel.OnEventRaised += LookAt;
        }

        private void OnDisable()
        {
            _lookAtChannel.OnEventRaised -= LookAt;
        }

        private void LookAt(Vector3 direction)
        {
            // increase magnitude of direction so that it's very far
            direction = direction * 100;
            gameObject.transform.rotation = Quaternion.FromToRotation(gameObject.transform.position, direction);
        }
    }
}