using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;

namespace TerrariumXR.EventSystem
{
    [CreateAssetMenu(menuName = "Events/Event Channels/PlanetState EventChannel", fileName = "PlanetStateEventChannel")]
    public class PlanetStateEventChannelSO : GenericEventChannelSO<PlanetState> {}
}