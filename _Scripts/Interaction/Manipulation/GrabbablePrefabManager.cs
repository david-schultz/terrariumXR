using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrariumXR.Interaction
{
    public class GrabbablePrefabManager : MonoBehaviour
    {
        [SerializeField] private Debugger _debugger;
        // [SerializeField] private GrabbableGeometrySO _grabbableSO;
        [SerializeField] private GameObject _debugObjectPrefab;
        [SerializeField] private GameObject _vertexPrefab;
        [SerializeField] private GameObject _edgePrefab;
        [SerializeField] private GameObject _trianglePrefab;
        [SerializeField] private Transform _container;

        public void ClearChildren()
        {
            // _debugger.Log("Clearing children.");
            while (_container.childCount > 0)
            {
                Destroy(_container.GetChild(0).gameObject);
            }
        }

        public void AddGrabbable(string type, Vector3 position)
        {
            if (transform.childCount > 0) return;
            // _debugger.Log("Adding grabbable " + type + " at position: " + position.ToString());
            
            GameObject signifier;
            if (type == "vertex")
            {
                signifier = Instantiate(_vertexPrefab, _container);
            }
            else if (type == "edge")
            {
                signifier = Instantiate(_edgePrefab, _container);
            }
            else if (type == "triangle")
            {
                signifier = Instantiate(_trianglePrefab, _container);
            }
            else
            {
                _debugger.Log("Invalid type: " + type);
                return;
            }

            signifier.transform.Translate(position * _container.localScale.x);
        }



        // public Vector3 GetMidPoint(Vector3 v1, Vector3 v2)
        // {
        //     float x = ( v1.x +
        //                 v2.x ) / 2f;
        //     float y = ( v1.y +
        //                 v2.y ) / 2f;
        //     float z = ( v1.z +
        //                 v2.z ) / 2f;

        //     return new Vector3(x, y, z);
        // }

        // public Vector3 GetMidPoint(Vector3 v1, Vector3 v2, Vector3 v3)
        // {
        //     float x = ( v1.x +
        //                 v2.x + 
        //                 v3.x ) / 3f;
        //     float y = ( v1.y +
        //                 v2.y + 
        //                 v3.y ) / 3f;
        //     float z = ( v1.z +
        //                 v2.z + 
        //                 v3.z ) / 3f;

        //     return new Vector3(x, y, z);
        // }
    }
}