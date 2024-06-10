using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TerrariumXR.Interaction;
using TerrariumXR.EventSystem;
using FMODUnity;

namespace TerrariumXR.UI
{
    public class MainMenuManager : MonoBehaviour
    {
    // ================== References ==================
        [SerializeField] private Debugger _debugger;
        [SerializeField] private VoidEventChannelSO _toggleMainMenuEventChannel;
        [SerializeField] private StringStringEventChannelSO _menuSelectionEventChannel;

        [SerializeField] private GameObject rotateLeft;
        [SerializeField] private GameObject rotateRight;
        [SerializeField] private GameObject toggleDebug;
        [SerializeField] private GameObject resetPlanet;


        [SerializeField] private Transform planet;
        [SerializeField] private GameObject gizmos;
        [SerializeField] private PlanetStateSO _planetStateSO;
        [SerializeField] private VoidEventChannelSO _meshUpdateChannel;

        [SerializeField] private EventReference _uiTapSound;

        private GameObject canvas;
        private bool initialized = false;

        void Start()
        {
            canvas = transform.GetChild(0).gameObject;
            initialized = true;
        }

        private void OnEnable()
        {
            _toggleMainMenuEventChannel.OnEventRaised += ToggleMainMenu;
            _menuSelectionEventChannel.OnEventRaised += HandleMenuUpdate;
        }

        private void OnDisable()
        {
            _toggleMainMenuEventChannel.OnEventRaised -= ToggleMainMenu;
            _menuSelectionEventChannel.OnEventRaised -= HandleMenuUpdate;
        }

    // ================== Functions ==================

        private void ToggleMainMenu()
        {
            _debugger.Log("toggling main menu");
            if (canvas.activeSelf)
            {
                canvas.SetActive(false);
            }
            else
            {
                canvas.SetActive(true);
            }
        }
        
    // ================== Functions ==================

        private void HandleMenuUpdate(string menuName, string state)
        {
            if (state == "inactive")
            {
                HandleHover(menuName, false);
            }
            if (state == "hover")
            {
                HandleHover(menuName, true);
            }
            if (state == "selected")
            {
                HandleTap(menuName);
            }
        }


        public void HandleHover(string menuName, bool isHovering)
        {
            // _debugger.Log("detected hover");
            if (menuName == "RotateLeft45")
            {
                if (isHovering)
                {
                    rotateLeft.GetComponent<TextMeshProUGUI>().color = new Color32(87,217,191,255);
                }
                else
                {
                    rotateLeft.GetComponent<TextMeshProUGUI>().color = new Color32(255,255,255,255);
                }
            }
            if (menuName == "RotateRight45")
            {
                if (isHovering)
                {
                    rotateRight.GetComponent<TextMeshProUGUI>().color = new Color32(87,217,191,255);
                }
                else
                {
                    rotateRight.GetComponent<TextMeshProUGUI>().color = new Color32(255,255,255,255);
                }
            }
            if (menuName == "ToggleDebug")
            {
                if (isHovering)
                {
                    toggleDebug.GetComponent<TextMeshProUGUI>().color = new Color32(87,217,191,255);
                }
                else
                {
                    toggleDebug.GetComponent<TextMeshProUGUI>().color = new Color32(255,255,255,255);
                }
            }
            if (menuName == "ResetPlanet")
            {
                if (isHovering)
                {
                    resetPlanet.GetComponent<TextMeshProUGUI>().color = new Color32(87,217,191,255);
                }
                else
                {
                    resetPlanet.GetComponent<TextMeshProUGUI>().color = new Color32(255,255,255,255);
                }
            }
        }

        public void HandleTap(string menuName)
        {
            // _debugger.Log("detected tap");
            if (menuName == "RotateLeft45")
            {
                planet.Rotate(Vector3.up, -45);
                AudioManager.instance.PlayOneShot(_uiTapSound, transform.position);
            }
            if (menuName == "RotateRight45")
            {
                planet.Rotate(Vector3.up, 45);
                AudioManager.instance.PlayOneShot(_uiTapSound, transform.position);
            }
            if (menuName == "ToggleDebug")
            {
                if (gizmos.activeSelf)
                {
                    gizmos.SetActive(false);
                }
                else
                {
                    gizmos.SetActive(true);
                }
                AudioManager.instance.PlayOneShot(_uiTapSound, transform.position);
            }
            if (menuName == "ResetPlanet")
            {
                _planetStateSO.Initialize("Timber Hearth", 0.25f, 3, false);
                _meshUpdateChannel?.RaiseEvent();
                AudioManager.instance.PlayOneShot(_uiTapSound, transform.position);
            }
        }

        
 
    }
}