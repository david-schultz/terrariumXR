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
    public class OnboardingMenuManager : MonoBehaviour
    {
    // ================== References ==================
        [SerializeField] private Debugger _debugger;
        [SerializeField] private StringStringEventChannelSO _menuSelectionEventChannel;

        [SerializeField] private GameObject onboardingCanvas;
        [SerializeField] private GameObject questionMarkIcon;
        [SerializeField] private GameObject boxCollider;

        [SerializeField] private TextMeshProUGUI pageIndicator;

        [SerializeField] private EventReference _uiTapSound;

        private GameObject canvas;
        private bool initialized = false;
        private string prevPage = "Page1";
        private string curPage = "Page1";
        private string nextPage = "Page2";

        void Start()
        {
            canvas = transform.GetChild(0).gameObject;
            initialized = true;
        }

        private void OnEnable()
        {
            _menuSelectionEventChannel.OnEventRaised += HandleMenuInput;
        }

        private void OnDisable()
        {
            _menuSelectionEventChannel.OnEventRaised -= HandleMenuInput;
        }
        
    // ================== Functions ==================

        private void HandleMenuInput(string menuName, string state)
        {
            if (!initialized) return;

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
            if (!initialized) return;
            // _debugger.Log("detected hover");
            if (menuName == "ToggleOnboarding")
            {
                if (isHovering)
                {
                    questionMarkIcon.GetComponent<RawImage>().color = new Color32(255,255,255,255);
                    boxCollider.GetComponent<MeshRenderer>().enabled = true;
                }
                else
                {
                    questionMarkIcon.GetComponent<RawImage>().color = new Color32(255,255,255,125);
                    boxCollider.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }

        public void HandleTap(string menuName)
        {
            if (!initialized) return;

            if (menuName == "ToggleOnboarding")
            {
                if (onboardingCanvas.activeSelf)
                {
                    onboardingCanvas.SetActive(false);
                    AudioManager.instance.PlayOneShot(_uiTapSound, transform.position);
                }
                else
                {
                    onboardingCanvas.SetActive(true);
                    OpenPage("Page1");
                    AudioManager.instance.PlayOneShot(_uiTapSound, transform.position);
                }
            }

            if (menuName == "CloseOnboarding")
            {
                onboardingCanvas.SetActive(false);
                AudioManager.instance.PlayOneShot(_uiTapSound, transform.position);
            }

            if (menuName == "PrevPage")
            {
                AudioManager.instance.PlayOneShot(_uiTapSound, transform.position);
                OpenPage(prevPage);
            }
            if (menuName == "NextPage")
            {
                AudioManager.instance.PlayOneShot(_uiTapSound, transform.position);
                OpenPage(nextPage);
            }
        }

        public void OpenPage(string pageName)
        {
            if (!initialized) return;

            List<GameObject> pages = new List<GameObject>();
            for (int i = 1; i < 9; i++)
            {
                GameObject page = onboardingCanvas.transform.Find("Page" + i).gameObject;
                pages.Add(page);
            }

            HideAll(pages);

            if (pageName == "Page1")
            {
                prevPage = "Page1";
                curPage = "Page1";
                nextPage = "Page2";
                pages[0].SetActive(true);
                pageIndicator.text = "1/8";
            }

            if (pageName == "Page2")
            {
                prevPage = "Page1";
                curPage = "Page2";
                nextPage = "Page3";
                pages[1].SetActive(true);
                pageIndicator.text = "2/8";
            }

            if (pageName == "Page3")
            {
                prevPage = "Page2";
                curPage = "Page3";
                nextPage = "Page4";
                pages[2].SetActive(true);
                pageIndicator.text = "3/8";
            }

            if (pageName == "Page4")
            {
                prevPage = "Page3";
                curPage = "Page4";
                nextPage = "Page5";
                pages[3].SetActive(true);
                pageIndicator.text = "4/8";
            }

            if (pageName == "Page5")
            {
                prevPage = "Page4";
                curPage = "Page5";
                nextPage = "Page6";
                pages[4].SetActive(true);
                pageIndicator.text = "5/8";
            }

            if (pageName == "Page6")
            {
                prevPage = "Page5";
                curPage = "Page6";
                nextPage = "Page7";
                pages[5].SetActive(true);
                pageIndicator.text = "6/8";
            }

            if (pageName == "Page7")
            {
                prevPage = "Page6";
                curPage = "Page7";
                nextPage = "Page8";
                pages[6].SetActive(true);
                pageIndicator.text = "7/8";
            }

            if (pageName == "Page8")
            {
                prevPage = "Page7";
                curPage = "Page8";
                nextPage = "Page8";
                pages[7].SetActive(true);
                pageIndicator.text = "8/8";
            }
        }

        public void HideAll(List<GameObject> pages)
        {
            foreach (GameObject page in pages)
            {
                page.SetActive(false);
            }
        }
 
    }
}