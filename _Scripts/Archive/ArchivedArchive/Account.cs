using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellarBody;

namespace Accounts {
    public class Account
    {
        private int _accountId;
        private string _username;
        private int _galaxyId;

        #region Properties
        public int id
        {
            get => _accountId;
        }
        public string username
        {
            get => _username;
        }
        public int galaxyId
        {
            get => _galaxyId;
            set => _galaxyId = value;
        }
        #endregion

        public Account(string username)
        {
            _accountId = username.GetHashCode();
            _username = username;
            _galaxyId = -1;
        }

        // public void AssignGalaxy(int galaxyId)
        // {
        //     _galaxyId = galaxyId;
        // }
    }
}