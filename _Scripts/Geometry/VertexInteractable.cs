using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR
{
    public class VertexInteractable : MonoBehaviour
    {
        // [SerializeField] private Material _baseMaterial;
        // [SerializeField] private Material _highlightMaterial;

        private Vector3 normal;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Highlight()
        {
            // "_Color" is the main color of a material.
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow); 
        }

        public void Unhighlight()
        {
            // "_Color" is the main color of a material.
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.grey); 
        }
    }
}