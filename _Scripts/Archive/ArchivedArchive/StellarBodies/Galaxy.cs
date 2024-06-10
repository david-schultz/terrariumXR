using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StellarBody {
    public class Galaxy : StellarBody, IGalaxy
    {
        // private List<int> _starIds;

        #region Properties
        public override string type
        {
            get => "Galaxy";
        }
        #endregion

        public Galaxy(int parentId) : base(parentId)
        {

        }

    }
}
