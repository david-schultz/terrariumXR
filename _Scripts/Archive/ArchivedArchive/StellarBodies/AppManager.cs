using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StellarBody;
using Accounts;

public class AppManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private StellarBodyManager _stellarBodyManager;
    [SerializeField] private AccountManager _accountManager;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _centerEyeAnchor;

    [Header("Prefabs")]
    [SerializeField] private GameObject _galaxyViewPrefab;
    [SerializeField] private GameObject _starViewPrefab;
    [SerializeField] private GameObject _planetViewPrefab;

    // [Header("Options")]
    // [SerializeField] private bool applyPreset = true;

    private int currentAccountId;
    private StellarObjectView currentView;
    private int currentViewId;
    private string currentViewType;
    private int currentSelectionId;

    // [SerializeField] private Transform initialPosition;


    // ========================================================================================
    // Initialization
    // ========================================================================================

    void Awake()
    {
        // if (applyPreset)
        // {
        //     LoadPreset();
        //     // LoadObjects();
        // }
    }

    public void LoadPreset()
    {
        Account alice = CreateAccount("Alice");
        // Account bob = CreateAccount("Bob");

        int galaxyAId = alice.galaxyId;
        // int galaxyBId = bob.galaxyId;
        for (int i = 0; i < 3; i++)
        {
            int starAId = _stellarBodyManager.Create("Star", galaxyAId);
            // int starBId = _stellarBodyManager.Create("Star", galaxyBId);
            for (int j = 0; j < 3; j++)
            {
                int planetAId = _stellarBodyManager.Create("Planet", starAId);
                // int planetBId = _stellarBodyManager.Create("Planet", starBId);
                for (int k = 0; k < 3; k++)
                {
                    _stellarBodyManager.Create("Flag", planetAId);
                    // _stellarBodyManager.Create("Flag", planetBId);
                }
            }
        }
    }


    // ========================================================================================
    // Account mgmt
    // ========================================================================================

    public Account CreateAccount(string username)
    {
        Account account = _accountManager.CreateAccount(username);
        LoadAccount(account);
        return account;
    }

    public void LoadAccount(int id)
    {
        LoadAccount(_accountManager.Get(id));
    }

    public void LoadAccount(string username)
    {
        LoadAccount(_accountManager.Get(username));
    }

    private void LoadAccount(Account account)
    {
        currentAccountId = account.id;
        currentViewId = account.galaxyId;
        currentSelectionId = account.galaxyId;
        currentViewType = "Galaxy";
    }

    public void SwitchAccount()
    {
        foreach (int id in _accountManager.GetAccountIds())
        {
            if (id != currentAccountId)
            {
                LoadAccount(id);
                return;
            }
        }
    }

    public Account GetCurrentAccount()
    {
        if (currentAccountId == -1) return null;
        return _accountManager.Get(currentAccountId);
    }

    public int GetNumObjectsInAccount(int accountId)
    {
        if (accountId == -1) return -1;
        int galaxyId = _accountManager.Get(accountId).galaxyId;
        return _stellarBodyManager.GetNumberOfChildren(galaxyId);
    }

    public int GetNumObjects(int stellarBodyId)
    {
        return _stellarBodyManager.GetNumberOfChildren(stellarBodyId);
    }


    // ========================================================================================
    // GameObject management functions
    // ========================================================================================

    public void LoadObjects()
    {
        // ClearObjects();
        switch (currentViewType)
        {
            case "Galaxy":
                LoadView(_galaxyViewPrefab);
                break;
            case "Star":
                LoadView(_starViewPrefab);
                break;
            case "Planet":
                LoadView(_planetViewPrefab);
                break;
        }
    }

    // Loads/instantiates the currentViewId, based off the currentViewType
    private void LoadView(GameObject prefab)
    {
        ClearObjects();
        IStellarBody stellarBody = _stellarBodyManager.Get(currentViewId);
        
        // StellarObjectView view = Instantiate(prefab, _container).GetComponent<StellarObjectView>();
        // StellarObjectView view = Instantiate(prefab).GetComponent<StellarObjectView>();
        GameObject obj = Instantiate(prefab);
        obj.transform.SetParent(_container.transform);
        obj.transform.localPosition = new Vector3(0, 0, 0);

        currentView = obj.GetComponent<StellarObjectView>();
        currentView.Initialize(currentViewId, stellarBody.GetChildren());

        currentSelectionId = currentViewId;
    }

    public void Unload()
    {
        currentAccountId = -1;
        currentView = null;
        currentViewId = -1;
        currentViewType = "";
        currentSelectionId = -1;
        ClearObjects();
    }

    private void ClearObjects()
    {
        foreach (Transform child in _container)
        {
            Destroy(child.gameObject);
        }
    }

    // ========================================================================================
    // Navigation functions
    // ========================================================================================

    public void SetCurrentView(int id)
    {
        currentViewId = id;
        currentViewType = _stellarBodyManager.Get(id).type;
        // LoadObjects();
    }

    public void UpdateSelection(int id)
    {
        currentSelectionId = id;
    }

    public IStellarBody GetSelection()
    {
        return _stellarBodyManager.Get(currentSelectionId);
    }

    public void SetPosition()
    {
        Vector3 cameraPosition = _centerEyeAnchor.position;
        Vector3 moveForward = _centerEyeAnchor.transform.forward * 0.5f;
        Vector3 moveDown = new Vector3(0, -0.25f, 0);
        _container.transform.localPosition = cameraPosition + moveForward + moveDown;
    }

    public void CreateChild()
    {
        string newType = "Star";
        switch (currentViewType)
        {
            case "Galaxy":
                newType = "Star";
                break;
            case "Star":
                newType = "Planet";
                break;
            case "Planet":
                newType = "Flag";
                break;
        }

        int childId = _stellarBodyManager.Create(newType, currentSelectionId);
        currentView.InstantiateChild(childId);
        UpdateSelection(childId);
        currentView.Select(childId);
    }

    public void RemoveSelection()
    {
        if (currentSelectionId == currentViewId) return;
        _stellarBodyManager.Remove(currentSelectionId);
        currentView.Remove(currentSelectionId);

        UpdateSelection(currentViewId);
        currentView.Select(currentViewId);
    }



    // ========================================================================================
    // Other
    // ========================================================================================

    public IStellarBody GetCurrentBody()
    {
        if (currentViewId == -1) return null;
        return _stellarBodyManager.Get(currentViewId);
    }

    public StellarObjectView GetCurrentView()
    {
        return currentView;
    }

    public IStellarBody GetBody(int id)
    {
        return _stellarBodyManager.Get(id);
    }

    public string GetCurrentType()
    {
        return currentViewType;
    }

}