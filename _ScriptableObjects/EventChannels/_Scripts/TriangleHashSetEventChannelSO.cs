using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR.Geometry;

namespace TerrariumXR.EventSystem
{
    [CreateAssetMenu(menuName = "Events/Event Channels/TriangleHashSet EventChannel", fileName = "TriangleHashSetEventChannel")]
    public class TriangleHashSetEventChannelSO : GenericEventChannelSO<TriangleHashSet> {}
}