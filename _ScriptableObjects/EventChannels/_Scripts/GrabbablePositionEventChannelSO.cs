using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR.Interaction;

namespace TerrariumXR.EventSystem
{
    [CreateAssetMenu(menuName = "Events/Event Channels/GrabbablePosition EventChannel", fileName = "GrabbablePositionEventChannel")]
    public class GrabbablePositionEventChannelSO : GenericEventChannelSO<IGrabbablePositionData> {}
}