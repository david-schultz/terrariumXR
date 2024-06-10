using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.EventSystem
{
    [CreateAssetMenu(menuName = "Events/Event Channels/Vector3 EventChannel", fileName = "Vector3EventChannel")]
    public class Vector3EventChannelSO : GenericEventChannelSO<Vector3> {}
}