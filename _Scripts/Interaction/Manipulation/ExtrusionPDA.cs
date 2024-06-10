using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Surfaces;
using TerrariumXR.Geometry;
using TerrariumXR.EventSystem;
using TerrariumXR.UI;

namespace TerrariumXR
{
    public class ExtrusionPDA : MonoBehaviour
    {
    // ================== Scriptable Objects ==================
        [SerializeField] private GameStatusSO _gameSO;
        [SerializeField] private SteppedSliderSO _sliderSO;

    // ================== Event Broadcasting ==================
        [SerializeField] private Debugger _debugger;
        [SerializeField] private PDAUILoader _uiLoader;
        [SerializeField] private StringEventChannelSO _gamemodeChannel;
        [SerializeField] private VoidEventChannelSO _sliderUpdateChannel;
        [SerializeField] private IntEventChannelSO _triangleSelectionChannel; 
        [SerializeField] private VoidEventChannelSO _extrudeChannel;

    // ================== Variables ==================
        [SerializeField] private GameObject _container;


        private void Start()
        {
            _uiLoader.Initialize(_gameSO, _sliderSO);

            ToggleActive(_gameSO.Gamemode);
        }

        private void OnEnable()
        {
            _gamemodeChannel.OnEventRaised += ToggleActive;
            _sliderUpdateChannel.OnEventRaised += SliderUpdate;
            _triangleSelectionChannel.OnEventRaised += SelectionUpdate;
        }

        private void OnDisable()
        {
            _gamemodeChannel.OnEventRaised -= ToggleActive;
            _sliderUpdateChannel.OnEventRaised -= SliderUpdate;
            _triangleSelectionChannel.OnEventRaised -= SelectionUpdate;
        }


        private void ToggleActive(string gamemode)
        {
            // _debugger.Log("toggle active");
            if (gamemode == "view")
            {
                _container.SetActive(true);
                _uiLoader.LoadSelection();
            }
            else
            {
                _container.SetActive(false);
                // _uiLoader.LoadSelection();
            }
        }

        // • track position of grabbable
        // • show ui for each selected triangle
        private void SliderUpdate()
        {
            _uiLoader.UpdateKnobPositionIndicator();

            _extrudeChannel?.RaiseEvent();

            // ICommand command = new ExtrudeCommand();
            // _commandChannel?.RaiseEvent(command);
        }

        private void SelectionUpdate(int triangleId)
        {
            _uiLoader.LoadSelection();
        }
    }
}