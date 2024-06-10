using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;

namespace TerrariumXR.EventSystem
{
    [CreateAssetMenu(menuName = "Events/Event Channels/GameState EventChannel", fileName = "GameStateEventChannel")]
    public class GameStateEventChannelSO : GenericEventChannelSO<GameState> {}
}