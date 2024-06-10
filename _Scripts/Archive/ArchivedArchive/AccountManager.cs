using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellarBody;
using TMPro;

namespace Accounts {
    public class AccountManager : MonoBehaviour
    {
        [SerializeField] private StellarBodyManager _stellarBodyManager;

        private Dictionary<int, Account> _accounts; // account id, account name

        void Awake()
        {
            _accounts = new Dictionary<int, Account>();
        }

    // ========================================================================================
    // Account functions
    // ========================================================================================

        public Account CreateAccount(string username)
        {
            if (Get(username) != null) return null;
            Account account = new Account(username);
            _accounts.Add(account.id, account);
            
            account.galaxyId = _stellarBodyManager.Create("Galaxy", account.id);

            return account;
        }

        public Account Get(int id)
        {
            return _accounts[id];
        }

        public Account Get(string username)
        {
            foreach (Account account in _accounts.Values)
            {
                if (account.username == username)
                {
                    return account;
                }
            }
            return null;
        }

        public List<int> GetAccountIds()
        {
            return new List<int>(_accounts.Keys);
        }

        public int GetNumberOfAccounts()
        {
            return _accounts.Count;
        }

        // public List<string> GetAccountUsernames()
        // {
        //     List<string> usernames = new List<string>();
        //     foreach (Account account in _accounts.Values)
        //     {
        //         usernames.Add(account.username);
        //     }
        //     return usernames;
        // }
    }
}

