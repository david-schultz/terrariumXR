using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.EventSystem
{
    [CreateAssetMenu(menuName = "Events/Event Channels/Bool EventChannel", fileName = "BoolEventChannel")]
    public class BoolEventChannelSO : GenericEventChannelSO<bool> {}
}