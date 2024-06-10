using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TerrariumXR.EventSystem
{
    public class EventListener: MonoBehaviour
    {
        [SerializeField]
        private VoidEventChannelSO m_EventChannel;

        private void OnEnable()
        {
            m_EventChannel.OnEventRaised += HandleEvent;
        }

        private void OnDisable()
        {
            m_EventChannel.OnEventRaised -= HandleEvent;
        }

        private void HandleEvent()
        {
            Debug.Log("Event received");
        }
    }
}