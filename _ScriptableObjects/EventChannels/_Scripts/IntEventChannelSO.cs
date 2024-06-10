using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.EventSystem
{
    [CreateAssetMenu(menuName = "Events/Event Channels/Int EventChannel", fileName = "IntEventChannel")]
    public class IntEventChannelSO : GenericEventChannelSO<int> {}
}