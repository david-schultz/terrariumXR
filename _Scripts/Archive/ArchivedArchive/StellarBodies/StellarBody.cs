using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StellarBody
{
    public abstract class StellarBody : IStellarBody
    {
        private int _id;
        private int _parentId;
        private string _name;
        private List<int> _children;
        

        #region Properties

        public int id
        {
            get => _id;
        }
        
        public int parentId
        {
            get => _parentId;
            set => _parentId = value;
        }

        public virtual string type
        {
            get => "Generic";
        }

        public string name
        {
            get => _name;
            set => _name = value;
        }
        #endregion

        public StellarBody(int parentId)
        {
            _name = "unnamed";
            // _id = name.GetHashCode() + parentId;

            _id = Random.Range(1, 10000) + parentId;

            _parentId = parentId;
            _children = new List<int>();
        }

        public List<int> GetChildren()
        {
            return _children;
        }

        public void AddChild(int id)
        {
            _children.Add(id);
        }
    }
}