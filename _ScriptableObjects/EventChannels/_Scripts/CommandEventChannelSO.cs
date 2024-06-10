using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.EventSystem
{
    [CreateAssetMenu(menuName = "Events/Event Channels/Command EventChannel", fileName = "CommandEventChannel")]
    public class CommandEventChannelSO : GenericEventChannelSO<ICommand> {}
}