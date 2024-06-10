using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TerrariumXR.Interaction;
using TerrariumXR.EventSystem;

namespace TerrariumXR.UI
{
    public class HandIndicatorManager : MonoBehaviour
    {
    // ================== References ==================
        [SerializeField] private Debugger _debugger;
        [SerializeField] private HandPoseWatcher _poseWatcher;
        [SerializeField] private BoolStringEventChannelSO _handPoseEventChannel;
        [SerializeField] private StringStringEventChannelSO _menuSelectionEventChannel;
        private TextMeshProUGUI textMesh;
        private GameObject leftFilled;
        private GameObject leftOutlined;
        private GameObject rightFilled;
        private GameObject rightOutlined;

        private bool initialized = false;

    // ================== Variables ==================
        private bool isLeftPrimary;
        private string activePose;

    // ================== Event System ==================
        void Awake()
        {
            textMesh = GameObject.Find("ActivePoseText").GetComponent<TextMeshProUGUI>();
            leftFilled = GameObject.Find("icon-hand-left-filled");
            leftOutlined = GameObject.Find("icon-hand-left-outlined");
            rightFilled = GameObject.Find("icon-hand-right-filled");
            rightOutlined = GameObject.Find("icon-hand-right-outlined");
            // isLeftPrimary = false;
            // activePose = "VertexSelector";

            initialized = true;

            UpdateIcons(false);
            UpdateText("VertexSelector");
        }

        private void OnEnable()
        {
            _handPoseEventChannel.OnEventRaised += UpdateUI;
            _menuSelectionEventChannel.OnEventRaised += HandleMenuUpdate;
        }

        private void OnDisable()
        {
            _handPoseEventChannel.OnEventRaised -= UpdateUI;
            _menuSelectionEventChannel.OnEventRaised -= HandleMenuUpdate;
        }

        private void HandleMenuUpdate(string menuName, string state)
        {
            if (menuName == "LeftHandButton" || menuName == "RightHandButton")
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
        }

        public void HandleHover(string menuName, bool isHovering)
        {
            // _debugger.Log("detected hover");
            if (menuName == "LeftHandButton")
            {
                if (isHovering)
                {
                    leftFilled.GetComponent<Image>().color = new Color32(87,217,191,255);
                    leftOutlined.GetComponent<Image>().color = new Color32(87,217,191,255);
                }
                else
                {
                    leftFilled.GetComponent<Image>().color = new Color32(255,255,255,255);
                    leftOutlined.GetComponent<Image>().color = new Color32(255,217,255,255);
                }
            }
            if (menuName == "RightHandButton")
            {
                if (isHovering)
                {
                    rightFilled.GetComponent<Image>().color = new Color32(87,217,191,255);
                    rightOutlined.GetComponent<Image>().color = new Color32(87,217,191,255);
                }
                else
                {
                    rightFilled.GetComponent<Image>().color = new Color32(255,255,255,255);
                    rightOutlined.GetComponent<Image>().color = new Color32(255,217,255,255);
                }
            }
        }

        public void HandleTap(string menuName)
        {
            // _debugger.Log("detected tap");
            if (menuName == "LeftHandButton")
            {
                if (isLeftPrimary)
                {
                    CyclePoses();
                    // _handPoseEventChannel?.RaiseEvent(isLeftPrimary, activePose);
                }
                else
                {
                    _poseWatcher.SetPrimaryHand(true);
                    // _handPoseEventChannel?.RaiseEvent(isLeftPrimary, "VertexSelector");
                    // UpdateIcons(true);
                }
            }
            if (menuName == "RightHandButton")
            {
                if (!isLeftPrimary)
                {
                    CyclePoses();
                    // _handPoseEventChannel?.RaiseEvent(isLeftPrimary, activePose);
                }
                else
                {
                    _poseWatcher.SetPrimaryHand(false);
                    // _handPoseEventChannel?.RaiseEvent(isLeftPrimary, "VertexSelector");
                    // UpdateIcons(false);
                }
            }
        }
        
    // ================== Functions ==================
        private void UpdateUI(bool isLeft, string pose)
        {
            UpdateIcons(isLeft);
            UpdateText(pose);
        }

        private void UpdateIcons(bool isLeft)
        {
            if (!initialized) return;
            if (isLeft)
            {
                leftFilled.SetActive(true);
                rightFilled.SetActive(false);
                leftOutlined.SetActive(false);
                rightOutlined.SetActive(true);
            }
            else
            {
                leftFilled.SetActive(false);
                rightFilled.SetActive(true);
                leftOutlined.SetActive(true);
                rightOutlined.SetActive(false);
            }

            isLeftPrimary = isLeft;
        }

        private void UpdateText(string pose)
        {
            if (!initialized) return;

            textMesh.text = pose;
            activePose = pose;
        }

        private void CyclePoses()
        {
            if (activePose == "VertexSelector")
            {
                activePose = "EdgeSelector";
                _poseWatcher.SetEdgeSelector();
            }
            else if (activePose == "EdgeSelector")
            {
                activePose = "TriangleSelector";
                _poseWatcher.SetTriangleSelector();
            }
            else if (activePose == "TriangleSelector")
            {
                activePose = "VertexSelector";
                _poseWatcher.SetVertexSelector();
            }

            UpdateText(activePose);
        }
    }
}