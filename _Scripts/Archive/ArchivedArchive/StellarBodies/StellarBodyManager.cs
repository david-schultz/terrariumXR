using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Accounts;

namespace StellarBody
{
    public class StellarBodyManager : MonoBehaviour
    {
        [SerializeField] private AccountManager _accountManager;
        private Dictionary<int, IStellarBody> _stellarBodies;

        void Awake()
        {
            _stellarBodies = new Dictionary<int, IStellarBody>();
        }




    // ========================================================================================
    // Create functions
    // ========================================================================================

        public int Create(string type, int parentId)
        {
            int id = -1;
            switch (type)
            {
                case "Galaxy":
                    IGalaxy galaxy = new Galaxy(parentId);
                    _stellarBodies.Add(galaxy.id, galaxy);
                    id = galaxy.id;
                    break;
                case "Star":
                    IStar star = new Star(parentId);
                    _stellarBodies.Add(star.id, star);
                    _stellarBodies[parentId].AddChild(star.id);
                    id = star.id;
                    break;
                case "Planet":
                    IPlanet planet = new Planet(parentId);
                    _stellarBodies.Add(planet.id, planet);
                    _stellarBodies[parentId].AddChild(planet.id);
                    id = planet.id;
                    break;
                case "Flag":
                    IFlag flag = new Flag(parentId);
                    _stellarBodies.Add(flag.id, flag);
                    _stellarBodies[parentId].AddChild(flag.id);
                    id = flag.id;
                    break;
            }

            return id;
        }

        public void Remove(int id)
        {
            // remove all children
            IStellarBody parent;
            if (_stellarBodies.TryGetValue(id, out parent))
            {
                List<int> children = parent.GetChildren();
                foreach (int childId in children)
                {
                    _stellarBodies.Remove(childId);
                }

                _stellarBodies.Remove(parent.id);
            }
        }


    // ========================================================================================
    // Get functions
    // ========================================================================================

        public IStellarBody Get(int id)
        {
            return _stellarBodies[id];
        }

        // Below: RECURSION BABY!!
        public int GetNumberOfChildren(int id) {
            IStellarBody parent;
            if (_stellarBodies.TryGetValue(id, out parent))
            {
                int count = 1;
                List<int> children = parent.GetChildren();
                foreach (int childId in children)
                {
                    count += GetNumberOfChildren(childId);
                }
                
                return count;
            }
            else
            {
                return 0;
            }
        }


    }
}