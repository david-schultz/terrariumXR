
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Oculus.Interaction;

namespace TerrariumXR
{
    public class HandShapeDebugVisual : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Color _openColor = Color.red;
        [SerializeField] private Color _neutralColor = Color.yellow;
        [SerializeField] private Color _closedColor = Color.green;

        public Color OpenColor
        {
            get
            {
                return _openColor;
            }
            set
            {
                _openColor = value;
            }
        }

        public Color NeutralColor
        {
            get
            {
                return _neutralColor;
            }
            set
            {
                _neutralColor = value;
            }
        }

        public Color ClosedColor
        {
            get
            {
                return _closedColor;
            }
            set
            {
                _closedColor = value;
            }
        }

        private Material _material;
        protected bool _started = false;

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            this.AssertField(_renderer, nameof(_renderer));
            _material = _renderer.material;
            _material.color = _neutralColor;
            this.EndStart(ref _started);
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                SetOpen();
            }
        }

        private void OnDestroy()
        {
            Destroy(_material);
        }



        public void SetOpen()
        {
            if (_started)
            {
                _material.color = _openColor;
            }
        }

        public void SetNeutral()
        {
            if (_started)
            {
                _material.color = _neutralColor;
            }
        }

        public void SetClosed()
        {
            if (_started)
            {
                _material.color = _closedColor;
            }
        }

        #region Inject

        public void InjectRenderer(Renderer renderer)
        {
            _renderer = renderer;
        }

        #endregion
    }
}
