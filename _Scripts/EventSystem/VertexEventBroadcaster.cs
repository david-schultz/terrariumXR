using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.EventSystem
{
    public class VertexEventBroadcaster : MonoBehaviour
    {
        [SerializeField] private BoolEventChannelSO _vertexGrabbedChannel;
        [SerializeField] private Vector3EventChannelSO _vertexPositionChannel;

        public void VertexGrabbed()
        {
            _vertexGrabbedChannel?.RaiseEvent(true);
        }

        public void VertexReleased()
        {
            _vertexGrabbedChannel?.RaiseEvent(false);
        }

        public void VertexMoved(Vector3 position)
        {
            _vertexPositionChannel?.RaiseEvent(position);
        }
    }
}