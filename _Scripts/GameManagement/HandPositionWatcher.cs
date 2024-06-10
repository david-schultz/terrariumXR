using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR.Interaction;

namespace TerrariumXR.EventSystem
{
    public class HandPositionWatcher : MonoBehaviour
    {
        [SerializeField] private HandStateSO _handStateSO;
        [SerializeField] private Transform _leftHandTransform;
        [SerializeField] private Transform _rightHandTransform;

        private HandState _leftHand;
        private HandState _rightHand;

        void Start()
        {
            _leftHand = _handStateSO.LeftHand;
            _rightHand = _handStateSO.RightHand;
        }

        void Update()
        {
            _leftHand.Position = _leftHandTransform.position;
            _rightHand.Position = _rightHandTransform.position;
        }
    }
}