using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR.Interaction;

namespace TerrariumXR.EventSystem
{
    [CreateAssetMenu(menuName = "Events/Event Channels/VertexPositionData EventChannel", fileName = "VertexPositionDataEventChannel")]
    public class VertexPositionDataEventChannelSO : GenericEventChannelSO<IVertexPositionData> {}
}