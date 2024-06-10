using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;

namespace TerrariumXR.EventSystem
{
    [CreateAssetMenu(menuName = "Events/Event Channels/Vertex Position EventChannel", fileName = "VertexPositionEventChannel")]
    public class VertexPositionEventChannelSO : GenericGenericEventChannelSO<int, Vector3> {}
}