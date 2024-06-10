using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;

namespace TerrariumXR.EventSystem
{
    [CreateAssetMenu(menuName = "Events/Event Channels/Int Bool EventChannel", fileName = "IntBoolEventChannel")]
    public class IntBoolEventChannelSO : GenericGenericEventChannelSO<int, bool> {}
}