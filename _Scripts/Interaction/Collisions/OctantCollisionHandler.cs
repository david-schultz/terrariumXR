using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR.EventSystem;

namespace TerrariumXR.Interaction
{
    public class OctantCollisionHandler : MonoBehaviour
    {
    // ================== References ==================
        [SerializeField] private Debugger _debugger;
        [SerializeField] private HandStateSO _handStateSO;
        [SerializeField] private bool _displayWireframe = false;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _selectedMaterial;

    // ================== Event System ==================
        [SerializeField] private IntBoolEventChannelSO _octantCollisionChannel;

    // ================== Variables ==================
        [SerializeField] private int _octantId = -1;
        private bool _isLeftPrimary = false;
        
    // ================== Functions ==================
        void Start()
        {
            if (_displayWireframe)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                gameObject.GetComponent<MeshRenderer>().material = _defaultMaterial;
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }

    // TODO: alt/primary hand conditions
    // TODO: if hand is already in octant, don't raise event
        void OnTriggerEnter(Collider other)
        {
            // if (_gameSO.Gamemode != "geometry") return;
            if (!other.CompareTag("LeftHand") && !other.CompareTag("RightHand")) return;
            Debug.Log("Trigger entered...");

            if (other.CompareTag("LeftHand") && _handStateSO.LeftHand.IsPrimary)
            {
                _isLeftPrimary = true;
                RaiseTrue();
            }
            else if (other.CompareTag("RightHand") && _handStateSO.RightHand.IsPrimary)
            {
                _isLeftPrimary = false;
                RaiseTrue();
            }        
        }

        void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("LeftHand") && !other.CompareTag("RightHand")) return;
            if (_isLeftPrimary && _handStateSO.RightHand.IsPrimary)
            {
                _isLeftPrimary = false;
            }
            else if (!_isLeftPrimary && _handStateSO.LeftHand.IsPrimary)
            {
                _isLeftPrimary = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("LeftHand") && !other.CompareTag("RightHand")) return;
            if ( (other.CompareTag("LeftHand") && _isLeftPrimary) ||
                 (other.CompareTag("RightHand") && !_isLeftPrimary))
            {
                RaiseFalse();
            }
        }

        private void RaiseTrue()
        {
            if (_displayWireframe) gameObject.GetComponent<MeshRenderer>().material = _selectedMaterial;
            _octantCollisionChannel?.RaiseEvent(_octantId, true);
        }

        private void RaiseFalse()
        {
            if (_displayWireframe) gameObject.GetComponent<MeshRenderer>().material = _defaultMaterial;
            _octantCollisionChannel?.RaiseEvent(_octantId, false);
        }
    }
}