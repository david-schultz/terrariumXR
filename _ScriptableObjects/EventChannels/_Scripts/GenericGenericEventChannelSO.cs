using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TerrariumXR.EventSystem
{
    public abstract class GenericGenericEventChannelSO<T1, T2>: ScriptableObject
    {
        [Tooltip("The action to perform")]
        public UnityAction<T1, T2> OnEventRaised;

        public void RaiseEvent(T1 parameter1, T2 parameter2)
        {
            if (OnEventRaised == null)
                return;

            OnEventRaised.Invoke(parameter1, parameter2);
        }
    }
}