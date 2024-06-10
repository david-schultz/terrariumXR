using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;
using TerrariumXR.EventSystem;

namespace TerrariumXR.Geometry
{
    public class TriangleHighlighter : MonoBehaviour
    {
        [SerializeField] private GameStatusSO _gameStatusSO;

        [SerializeField] private Color _highlightColor;
        [SerializeField] private Transform _container;
        [SerializeField] private Transform _parent;

        [SerializeField] private PlanetStateEventChannelSO _highlightTriangleGroupChannel;
        [SerializeField] private IntEventChannelSO _highlightTriangleChannel;
        [SerializeField] private IntEventChannelSO _unhighlightTriangleChannel;

        private void OnEnable()
        {
            _highlightTriangleGroupChannel.OnEventRaised += UpdateGroup;
            _highlightTriangleChannel.OnEventRaised += Highlight;
            _unhighlightTriangleChannel.OnEventRaised += Unhighlight;
        }
        private void OnDisable()
        {
            _highlightTriangleGroupChannel.OnEventRaised -= UpdateGroup;
            _highlightTriangleChannel.OnEventRaised -= Highlight;
            _unhighlightTriangleChannel.OnEventRaised -= Unhighlight;
        }

        private void UpdateGroup(PlanetState planetState)
        {
            ClearAll();
        }

        private void Highlight(int triangleId)
        {
            
        }

        private void Unhighlight(int triangleId)
        {

        }

        private void ClearAll()
        {

        }
    }
}