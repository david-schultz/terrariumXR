using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.UI
{
    public class ToggleRotator : MonoBehaviour
    {
        [SerializeField] private GameObject _rotator;

        void OnTriggerEnter(Collider other)
        {
            // filter out non-hand objects
            if (!other.CompareTag("LeftHand") && !other.CompareTag("RightHand")) return;
            _rotator.SetActive(true);
        }
        void OnTriggerStay(Collider other)
        {
            // filter out non-hand objects
            if (!other.CompareTag("LeftHand") && !other.CompareTag("RightHand")) return;

        }
        void OnTriggerExit(Collider other)
        {
            // filter out non-hand objects
            if (!other.CompareTag("LeftHand") && !other.CompareTag("RightHand")) return;

            _rotator.SetActive(false);
        }
    }
}