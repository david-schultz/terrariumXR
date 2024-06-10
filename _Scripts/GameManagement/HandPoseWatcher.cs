using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using TerrariumXR.EventSystem;

namespace TerrariumXR.Interaction
{
    public class HandPoseWatcher : MonoBehaviour
    {
    // ================== References ==================
        [SerializeField] private Debugger _debugger;
        [SerializeField] private bool _debugMode = false;
        [SerializeField] private HandStateSO _handStateSO;
        [SerializeField] private BoolStringEventChannelSO _handPoseEventChannel;
        [SerializeField] private VoidEventChannelSO _toggleMainMenuEventChannel;
        [SerializeField] private BoolEventChannelSO _handTapEventChannel;
        [SerializeField] private EventReference _primaryHandChangedSound;
        [SerializeField] private EventReference _selectingPrimaryHandSound;
        [SerializeField] private EventReference _uiTapSound;
        [SerializeField] private EventReference _uiTapSound2;


    // ================== Event System ==================
        void Start()
        {
            if (_debugMode)
            {
                _debugger.Log("HandPoseWatcher started in debug mode.");
                SetPrimaryHand(false);
                SetVertexSelector(true);
            }
        }

        private void OnEnable()
        {
            _handPoseEventChannel.OnEventRaised += ReceivedEvent;
        }

        private void OnDisable()
        {
            _handPoseEventChannel.OnEventRaised -= ReceivedEvent;
        }
    
        private void ReceivedEvent(bool isLeftPrimary, string activePose)
        {
            if (!_handStateSO.isInitialized) return;

            SetPrimaryHand(isLeftPrimary);

            if (activePose == "VertexSelector")
            {
                SetVertexSelector(isLeftPrimary);
            }
            else if (activePose == "EdgeSelector")
            {
                SetEdgeSelector(isLeftPrimary);
            }
            else if (activePose == "TriangleSelector")
            {
                SetTriangleSelector(isLeftPrimary);
            }
        }
        
    // ================== Pose Handlers ==================
        public void SetPrimaryHand(bool isLeftHand)
        {
            if (isLeftHand)
            {
                _handStateSO.LeftHand.IsPrimary = true;
                _handStateSO.RightHand.IsPrimary = false;
                _handStateSO.LeftHand.ActivePose = "VertexSelector";
                AudioManager.instance.PlayOneShot(_primaryHandChangedSound, _handStateSO.LeftHand.Position);
            }
            else
            {
                _handStateSO.LeftHand.IsPrimary = false;
                _handStateSO.RightHand.IsPrimary = true;
                _handStateSO.RightHand.ActivePose = "VertexSelector";
                AudioManager.instance.PlayOneShot(_primaryHandChangedSound, _handStateSO.RightHand.Position);
            }

            _handPoseEventChannel?.RaiseEvent(_handStateSO.LeftHand.IsPrimary, _handStateSO.AltHand.ActivePose);
            _debugger.SetFieldData("FieldD|" + _handStateSO.PrimaryHand.Tag);
            _debugger.SetFieldData("FieldE|" + _handStateSO.AltHand.ActivePose);
        }

        public void SetVertexSelector()
        {
            _handStateSO.AltHand.ActivePose = "VertexSelector";
            AudioManager.instance.PlayOneShot(_uiTapSound, _handStateSO.AltHand.Position);

            _handPoseEventChannel?.RaiseEvent(_handStateSO.LeftHand.IsPrimary, _handStateSO.AltHand.ActivePose);
            _debugger.SetFieldData("FieldE|" + _handStateSO.AltHand.ActivePose);
        }

        public void SetEdgeSelector()
        {
            _handStateSO.AltHand.ActivePose = "EdgeSelector";
            AudioManager.instance.PlayOneShot(_uiTapSound, _handStateSO.AltHand.Position);

            _handPoseEventChannel?.RaiseEvent(_handStateSO.LeftHand.IsPrimary, _handStateSO.AltHand.ActivePose);
            _debugger.SetFieldData("FieldE|" + _handStateSO.AltHand.ActivePose);
        }
        
        public void SetTriangleSelector()
        {
            _handStateSO.AltHand.ActivePose = "TriangleSelector";
            AudioManager.instance.PlayOneShot(_uiTapSound, _handStateSO.AltHand.Position);

            _handPoseEventChannel?.RaiseEvent(_handStateSO.LeftHand.IsPrimary, _handStateSO.AltHand.ActivePose);
            _debugger.SetFieldData("FieldE|" + _handStateSO.AltHand.ActivePose);
        }

        public void SetVertexSelector(bool isLeftHand)
        {
            if (isLeftHand && !_handStateSO.LeftHand.IsPrimary)
            {
                _handStateSO.LeftHand.ActivePose = "VertexSelector";
                AudioManager.instance.PlayOneShot(_uiTapSound, _handStateSO.AltHand.Position);
            }
            else if (!isLeftHand && !_handStateSO.RightHand.IsPrimary)
            {
                _handStateSO.RightHand.ActivePose = "VertexSelector";
                AudioManager.instance.PlayOneShot(_uiTapSound, _handStateSO.AltHand.Position);
            }

            _handPoseEventChannel?.RaiseEvent(_handStateSO.LeftHand.IsPrimary, _handStateSO.AltHand.ActivePose);
            _debugger.SetFieldData("FieldE|" + _handStateSO.AltHand.ActivePose);

            // AudioManager.instance.PlayOneShot(_uiTapSound, _handStateSO.AltHand.Position);
        }

        public void SetEdgeSelector(bool isLeftHand)
        {
            if (isLeftHand && !_handStateSO.LeftHand.IsPrimary)
            {
                _handStateSO.LeftHand.ActivePose = "EdgeSelector";
                AudioManager.instance.PlayOneShot(_uiTapSound, _handStateSO.AltHand.Position);
            }
            else if (!isLeftHand && !_handStateSO.RightHand.IsPrimary)
            {
                _handStateSO.RightHand.ActivePose = "EdgeSelector";
                AudioManager.instance.PlayOneShot(_uiTapSound, _handStateSO.AltHand.Position);
            }

            _handPoseEventChannel?.RaiseEvent(_handStateSO.LeftHand.IsPrimary, _handStateSO.AltHand.ActivePose);
            _debugger.SetFieldData("FieldE|" + _handStateSO.AltHand.ActivePose);
            
            // AudioManager.instance.PlayOneShot(_uiTapSound, _handStateSO.AltHand.Position);

        }

        public void SetTriangleSelector(bool isLeftHand)
        {
            if (isLeftHand && !_handStateSO.LeftHand.IsPrimary)
            {
                _handStateSO.LeftHand.ActivePose = "TriangleSelector";
                AudioManager.instance.PlayOneShot(_uiTapSound, _handStateSO.AltHand.Position);
            }
            else if (!isLeftHand && !_handStateSO.RightHand.IsPrimary)
            {
                _handStateSO.RightHand.ActivePose = "TriangleSelector";
                AudioManager.instance.PlayOneShot(_uiTapSound, _handStateSO.AltHand.Position);
            }

            _handPoseEventChannel?.RaiseEvent(_handStateSO.LeftHand.IsPrimary, _handStateSO.AltHand.ActivePose);
            _debugger.SetFieldData("FieldE|" + _handStateSO.AltHand.ActivePose);

            // AudioManager.instance.PlayOneShot(_uiTapSound, _handStateSO.AltHand.Position);
        }

        public void BeginPrimaryHandSelection(bool isLeftHand)
        {
            if (isLeftHand && !_handStateSO.LeftHand.IsPrimary)
            {
                AudioManager.instance.PlayOneShot(_selectingPrimaryHandSound, _handStateSO.LeftHand.Position);
            }
            else if (!isLeftHand && !_handStateSO.RightHand.IsPrimary)
            {
                AudioManager.instance.PlayOneShot(_selectingPrimaryHandSound, _handStateSO.RightHand.Position);
            }

        }

        public void FingersTapped(bool isLeftHand)
        {
            _handTapEventChannel?.RaiseEvent(isLeftHand);
        }

        public void ToggleMainMenu()
        {
            _toggleMainMenuEventChannel?.RaiseEvent();
        }
    }
}