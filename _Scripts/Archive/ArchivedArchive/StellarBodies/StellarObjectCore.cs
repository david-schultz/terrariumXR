using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StellarBody
{
    public class StellarObjectCore : MonoBehaviour
    {
        [SerializeField] private OrbitalRotation _orbitalRotation;
        [SerializeField] private GameObject _outline;
        [SerializeField] private bool _isInteractable = true;
        private int stellarBodyId;
        // private bool selected;
        private bool _isChild;

        #region Properties
        public int Id
        {
            get => stellarBodyId;
        }
        public bool IsChild
        {
            get => _isChild;
        }
        #endregion


        public void Initialize(int id, bool isChild)
        {
            stellarBodyId = id;
            _isInteractable = true;
            // selected = false;
            _isChild = isChild;
        }

        public void Initialize(int id, bool isChild, Transform target)
        {
            stellarBodyId = id;
            _isInteractable = true;
            // selected = false;
            _isChild = isChild;

            _orbitalRotation.Target = target;
        }

        public void Hover()
        {
            if (!_isInteractable) return;
            // 1. change object outline
            // 2. show object name
        }

        // 1. show object details panel
        // 2. 
        public void Select()
        {
            // selected = true;
            _outline.SetActive(true);
        }

        public void Unselect()
        {
            // selected = false;
            _outline.SetActive(false);

        }
    }
}