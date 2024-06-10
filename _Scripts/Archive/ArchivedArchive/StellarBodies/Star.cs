using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StellarBody
{
    public class Star : StellarBody, IStar
    {

        #region Properties
        public override string type
        {
            get
            {
                return "Star";
            }
        }
        #endregion

        public Star(int parentId) : base(parentId)
        {
        }

    }
}
