using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TerrariumXR.EventSystem;

namespace TerrariumXR.UI
{
    public class MenuCollider : MonoBehaviour
    {
    // ================== References ==================
        [SerializeField] private StringStringEventChannelSO _menuSelectionEventChannel;
        [SerializeField] private BoolEventChannelSO _handTapEventChannel;
        [SerializeField] private string _menuName = "";
        [SerializeField] private bool _useNewFormat = false;
        
    // ================== Variables ==================
        private bool isLeftColliding = false;
        private bool isRightColliding = false;

    // ================== Event System ==================

        private void OnEnable()
        {
            _handTapEventChannel.OnEventRaised += HandleTap;
        }

        private void OnDisable()
        {
            _handTapEventChannel.OnEventRaised -= HandleTap;
        }

        void HandleTap(bool isLeftHand)
        {
            if ((isLeftColliding && isLeftHand) || (isRightColliding && !isLeftHand))
            {
                _menuSelectionEventChannel?.RaiseEvent(_menuName, "selected");
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("LeftHand"))
            {
                HandleHover();
                isLeftColliding = true;
                _menuSelectionEventChannel?.RaiseEvent(_menuName, "hover");
            }
            if (other.CompareTag("RightHand"))
            {
                HandleHover();
                isRightColliding = true;
                _menuSelectionEventChannel?.RaiseEvent(_menuName, "hover");
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("LeftHand"))
            {
                HandleUnhover();
                isLeftColliding = false;
                _menuSelectionEventChannel?.RaiseEvent(_menuName, "inactive");
            }
            if (other.CompareTag("RightHand"))
            {
                HandleUnhover();
                isRightColliding = false;
                _menuSelectionEventChannel?.RaiseEvent(_menuName, "inactive");
            }
        }

        void HandleHover()
        {
            if (!_useNewFormat) return;

            // get child 0 (main content)
            if (transform.GetChild(0) == null) return;
            GameObject textChild = transform.GetChild(0).gameObject;

            // if child is text
            if (textChild.GetComponent<TextMeshProUGUI>() != null)
            {
                textChild.GetComponent<TextMeshProUGUI>().color = new Color32(255,255,255,255);
            }
            // if child is image
            if (textChild.GetComponent<Image>() != null)
            {
                textChild.GetComponent<Image>().color = new Color32(255,255,255,255);
            }
            // if child is raw image
            if (textChild.GetComponent<RawImage>() != null)
            {
                textChild.GetComponent<RawImage>().color = new Color32(255,255,255,255);
            }

            // get child 1 (wireframe box)
            GameObject wireframeChild = transform.Find("Wireframe").gameObject;
            wireframeChild.SetActive(true);
        }

        void HandleUnhover()
        {
            if (!_useNewFormat) return;

            // get child 0 (main content)
            if (transform.GetChild(0) == null) return;
            GameObject textChild = transform.GetChild(0).gameObject;

            // if child is text
            if (textChild.GetComponent<TextMeshProUGUI>() != null)
            {
                textChild.GetComponent<TextMeshProUGUI>().color = new Color32(255,255,255,125);
            }
            // if child is image
            if (textChild.GetComponent<Image>() != null)
            {
                textChild.GetComponent<Image>().color = new Color32(255,255,255,125);
            }
            // if child is raw image
            if (textChild.GetComponent<RawImage>() != null)
            {
                textChild.GetComponent<RawImage>().color = new Color32(255,255,255,125);
            }

            // get child 1 (wireframe box)
            GameObject wireframeChild = transform.Find("Wireframe").gameObject;
            wireframeChild.SetActive(false);
        }
    }
}