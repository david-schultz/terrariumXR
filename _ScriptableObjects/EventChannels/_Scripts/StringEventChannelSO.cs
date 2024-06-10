using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.EventSystem
{
    [CreateAssetMenu(menuName = "Events/Event Channels/String EventChannel", fileName = "StringEventChannel")]
    public class StringEventChannelSO : GenericEventChannelSO<string> {}
}