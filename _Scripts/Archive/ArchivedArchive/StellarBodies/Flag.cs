using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StellarBody
{
    public class Flag : StellarBody, IFlag
    {
        private bool _isComplete;

        #region Properties
        public override string type
        {
            get
            {
                return "Flag";
            }
        }
        public bool isComplete
        {
            get
            {
                return _isComplete;
            }
        }
        #endregion

        public Flag(int parentId) : base(parentId)
        {
            MarkIncomplete();
        }

        public void MarkComplete()
        {
            _isComplete = true;
        }

        public void MarkIncomplete()
        {
            _isComplete = false;
        }


    }
}