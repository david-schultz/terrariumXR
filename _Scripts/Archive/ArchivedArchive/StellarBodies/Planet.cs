using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StellarBody
{
    public class Planet : StellarBody, IPlanet
    {
        #region Properties
        public override string type
        {
            get
            {
                return "Planet";
            }
        }
        #endregion

        public Planet(int parentId) : base(parentId)
        {
            
        }
    }
}