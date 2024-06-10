using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;

namespace TerrariumXR.EventSystem
{
    public class AudioEventListener : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _vertexHoverChannel;
        [SerializeField] private BoolEventChannelSO _vertexGrabChannel;

        [SerializeField] private AudioTrigger _hoverAudioTrigger;
        [SerializeField] private AudioTrigger _grabAudioTrigger;
        [SerializeField] private AudioTrigger _releaseAudioTrigger;

    // ================== Base ==================
        private void OnEnable()
        {
            _vertexGrabChannel.OnEventRaised += VertexGrabbed;
            _vertexHoverChannel.OnEventRaised += VertexHovered;
        }

        private void OnDisable()
        {
            _vertexGrabChannel.OnEventRaised -= VertexGrabbed;
            _vertexHoverChannel.OnEventRaised -= VertexHovered;
        }

    // ================== Functions ==================
        private void VertexGrabbed(bool isGrabbed)
        {
            if (isGrabbed)
            {
                _grabAudioTrigger.PlayAudio();
            }
            else
            {
                _releaseAudioTrigger.PlayAudio();
            }
        }

        private void VertexHovered()
        {
            _hoverAudioTrigger.PlayAudio();
        }

    }
}