using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using TerrariumXR.EventSystem;

namespace TerrariumXR.EventSystem
{
    public class ToggleSwipeMenu : MonoBehaviour
    {
        [SerializeField] private BoolEventChannelSO _toggleMenuChannel;
        [SerializeField] private Transform _handPosition;
        [SerializeField] private Transform _centerEyeAnchor;
        private Vector3 _targetPosition = new Vector3(0, 0, 0);

        private void OnEnable()
        {
            _toggleMenuChannel.OnEventRaised += Toggle;
        }

        private void OnDisable()
        {
            _toggleMenuChannel.OnEventRaised -= Toggle;
        }

        private void Toggle(bool isOn)
        {
            // 1. Set active
            gameObject.SetActive(isOn);
            if (isOn)
            {
                // 2. Set position by hand
                // 3. Lerp menu upwards
                SetStartPosition();
            }
        }

        void Start()
        {
            Toggle(false);
        }


        void Update()
        {
            if (Vector3.Distance(_targetPosition, transform.position) < 0.1f)
            {
                // TODO: IDK IF THIS WILL WORK PROPERLY
                _targetPosition = new Vector3(0, 0, 0);
            }
            else if (_targetPosition != new Vector3(0, 0, 0))
            {
                MoveUp();
            }
        }


        // handpos, look at player, + 0.5 forward, 0.25 down
        private void SetStartPosition()
        {
            // move to hand pos
            gameObject.transform.localPosition = _handPosition.localPosition;

            // look at the player
            transform.rotation = Quaternion.LookRotation(transform.position - _centerEyeAnchor.transform.position);

            // move forward + down
            Vector3 moveForward = _centerEyeAnchor.transform.forward * 0.5f;
            Vector3 moveDown = new Vector3(0, -0.25f, 0);
            gameObject.transform.localPosition += moveForward + moveDown;

            _targetPosition = _handPosition.localPosition + moveForward;
        }

        private void MoveUp()
        {
            transform.position += (_targetPosition - transform.position) * 0.025f;
        }


        // private Vector3 FindTargetPosition()
        // {
        //     // Let's get a position infront of the player's camera
        //     Vector3 xComponent = cameraTransform.right * xDistance;
        //     Vector3 yComponent = cameraTransform.up * yDistance;
        //     Vector3 zComponent = cameraTransform.forward * zDistance;
        //     return cameraTransform.position + xComponent + yComponent + zComponent;
        // }
    }
}