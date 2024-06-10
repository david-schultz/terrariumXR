using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;

namespace TerrariumXR.EventSystem
{
    [CreateAssetMenu(menuName = "Events/Event Channels/IntList EventChannel", fileName = "IntListEventChannel")]
    public class IntListEventChannelSO : GenericEventChannelSO<List<int>> {}
}