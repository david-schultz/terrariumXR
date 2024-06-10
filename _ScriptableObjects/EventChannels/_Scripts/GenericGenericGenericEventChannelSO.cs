using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TerrariumXR.EventSystem
{
    public abstract class GenericGenericGenericEventChannelSO<T1, T2, T3>: ScriptableObject
    {
        [Tooltip("The action to perform")]
        public UnityAction<T1, T2, T3> OnEventRaised;

        public void RaiseEvent(T1 parameter1, T2 parameter2, T3 parameter3)
        {
            // if (OnEventRaised == null)
            //     return;

            OnEventRaised?.Invoke(parameter1, parameter2, parameter3);
        }
    }
}