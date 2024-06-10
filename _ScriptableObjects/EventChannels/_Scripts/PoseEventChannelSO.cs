using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

namespace TerrariumXR.EventSystem
{
    [CreateAssetMenu(menuName = "Events/Event Channels/Pose EventChannel", fileName = "PoseEventChannel")]
    public class PoseEventChannelSO : GenericEventChannelSO<Pose> {}
}