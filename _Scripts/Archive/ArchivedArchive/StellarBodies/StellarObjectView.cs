using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StellarBody
{
    public class StellarObjectView : MonoBehaviour
    {
        [SerializeField] private GameObject _primaryCorePrefab;
        [SerializeField] private GameObject _childCorePrefab;
        [SerializeField] private Transform _childObjectsArea;
        [SerializeField] private float _scaleFactor = 1f;
        [SerializeField] private bool _place2D = true;
        private int stellarBodyId;
        private StellarObjectCore primaryCore;
        private List<StellarObjectCore> childCores;
        private bool isInitialized = false;
        private StellarObjectCore selectedCore;

        #region Properties
        public int Id
        {
            get => stellarBodyId;
        }
        #endregion


    // ========================================================================================
    // Initialization
    // ========================================================================================

        public void Initialize(int id, List<int> childIds)
        {
            stellarBodyId = id;
            InstantiatePrimaryCore(id);
            InstantiateChildCores(childIds);
            isInitialized = true;
        }

        private void InstantiatePrimaryCore(int id)
        {
            GameObject obj = Instantiate(_primaryCorePrefab, gameObject.transform);
            obj.transform.localScale = new Vector3(_scaleFactor, _scaleFactor, _scaleFactor);

            primaryCore = obj.GetComponent<StellarObjectCore>();
            primaryCore.Initialize(id, false);

            selectedCore = primaryCore;
            selectedCore.Select();
        }

        private void InstantiateChildCores(List<int> childIds)
        {
            childCores = new List<StellarObjectCore>();
            foreach (int childId in childIds)
            {
                InstantiateChild(childId);
            }
        }

        public void InstantiateChild(int childId)
        {
            GameObject obj = Instantiate(_childCorePrefab, _childObjectsArea);

            if (_place2D)
            {
                float radius = Random.Range(0.1f, 0.5f);
                int angle = Random.Range(0, 360);
                obj.transform.localPosition = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            }
            else
            {
                float radius = 0.14f;
                obj.transform.localPosition = Random.onUnitSphere * radius;

                Vector3 centerOfSphere = _childObjectsArea.position;
                Vector3 placementPosition = obj.transform.localPosition;
                Vector3 normal = (placementPosition - centerOfSphere).normalized;

                // Ray ray = new Ray(_childObjectsArea.position, obj.transform.localPosition);
                // RaycastHit hit;
                // if(Physics.Raycast(ray, out hit, Mathf.Infinity))
                // {
                //     obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                // }

                obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
                // obj.transform.rotation = Quaternion.LookRotation(normal, Vector3.up);
            }

            StellarObjectCore childCore = obj.GetComponent<StellarObjectCore>();
            childCore.Initialize(childId, true, primaryCore.transform);
            childCores.Add(childCore);
            childCore.Unselect();
        }


    // ========================================================================================
    // General functions
    // ========================================================================================

        public List<StellarObjectCore> GetChildren()
        {
            if (!isInitialized) return null;
            List<StellarObjectCore> childObjects = new List<StellarObjectCore>();
            foreach (Transform child in _childObjectsArea)
            {
                childObjects.Add(child.gameObject.GetComponent<StellarObjectCore>());
            }
            return childObjects;
        }

        public StellarObjectCore GetSelectedCore()
        {
            return selectedCore;
        }

        public void SelectNext()
        {
            if (!isInitialized) return;
            if (selectedCore == primaryCore)
            {
                selectedCore.Unselect();
                selectedCore = childCores[0];
                selectedCore.Select();
            }
            else
            {
                int index = childCores.IndexOf(selectedCore);
                selectedCore.Unselect();
                if (index == childCores.Count - 1)
                {
                    selectedCore = primaryCore;
                }
                else
                {
                    selectedCore = childCores[index + 1];
                }
                selectedCore.Select();
            }
        }

        public void SelectPrev()
        {
            if (!isInitialized) return;
            if (selectedCore == primaryCore)
            {
                selectedCore.Unselect();
                selectedCore = childCores[childCores.Count - 1];
                selectedCore.Select();
            }
            else
            {
                int index = childCores.IndexOf(selectedCore);
                selectedCore.Unselect();
                if (index == 0)
                {
                    selectedCore = primaryCore;
                }
                else
                {
                    selectedCore = childCores[index - 1];
                }
                selectedCore.Select();
            }
        }

        public void Select(int id)
        {
            if (!isInitialized) return;
            if (id == primaryCore.Id)
            {
                selectedCore.Unselect();
                selectedCore = primaryCore;
                selectedCore.Select();
            }
            else
            {
                StellarObjectCore targetCore = childCores.Find(core => core.Id == id);
                if (targetCore != null)
                {
                    selectedCore.Unselect();
                    selectedCore = targetCore;
                    selectedCore.Select();
                }
            }
        }

        public void Remove(int id)
        {
            if (!isInitialized) return;
            if (id == primaryCore.Id)
            {
                // Destroy(gameObject);
            }
            else
            {
                StellarObjectCore targetCore = childCores.Find(core => core.Id == id);
                if (targetCore != null)
                {
                    childCores.Remove(targetCore);
                    Destroy(targetCore.gameObject);
                }
            }
        }
    }
}