using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.UI
{
    public class MenuAreaCollider : MonoBehaviour
    {
    // ================== References ==================
        [SerializeField] private Debugger _debugger;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
            {
                _debugger.ClearDebug();
            }
        }
    }
}