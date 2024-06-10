using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Accounts;
using StellarBody;

public class ControlPanel : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private AppManager _appManager;

    [Header("Prefabs")]
    [SerializeField] private GameObject _galaxyCorePrefab;
    [SerializeField] private GameObject _starCorePrefab;
    [SerializeField] private GameObject _planetCorePrefab;
    [SerializeField] private GameObject _flagPrefab;

    [Header("Panels")]
    [SerializeField] private TextMeshProUGUI[] _textMeshes;
    [SerializeField] private TextMeshProUGUI[] _debugTextMeshes;
    [SerializeField] private GameObject _presetButton;
    [SerializeField] private GameObject _accountLoginPanel;
    [SerializeField] private GameObject _accountPanel;
    [SerializeField] private GameObject _inspectPanel;
    [SerializeField] private Transform _iconSlot;

    private StellarObjectView currentView;

    // 0: inspected object name
    // 1: inspected object id
    // 2: username
    // 3: account id
    // 4: number of objects

    // ========================================================================================
    // Inspect Functions
    // ========================================================================================

    public void InspectSelection()
    {
        IStellarBody body = _appManager.GetSelection();
        if (body == null)
        {
            _textMeshes[0].text = "[id]";
            _textMeshes[1].text = "[type]";
            _textMeshes[2].text = "[number of objects]";
        }
        else
        {
            _textMeshes[0].text = body.id.ToString();
            _textMeshes[1].text = body.type;
            _textMeshes[2].text = _appManager.GetNumObjects(body.id).ToString();
            SetIcon(body.type);
        }
    }

    public void InspectAccount()
    {
        Account account = _appManager.GetCurrentAccount();
        if (account == null)
        {
            _accountLoginPanel.SetActive(true);
            _accountPanel.SetActive(false);
        }
        else
        {
            _accountLoginPanel.SetActive(false);
            _accountPanel.SetActive(true);
            _textMeshes[3].text = account.username;
            _textMeshes[4].text = account.id.ToString();
            _textMeshes[5].text = _appManager.GetNumObjectsInAccount(_appManager.GetCurrentAccount().id).ToString();
        }
    }

    private void SetIcon(string type)
    {
        // Clear container
        foreach (Transform child in _iconSlot)
        {
            // Destroy(child.gameObject);
            child.gameObject.SetActive(false);
            if (type == child.gameObject.name)
            {
                child.gameObject.SetActive(true);
            }
        }
        
        // switch (type)
        // {
        //     case "Galaxy":
        //         child.gameObject.SetActive(false);
        //         // Instantiate(_galaxyCorePrefab, _iconSlot);
        //         break;
        //     case "Star":
        //         // Instantiate(_starCorePrefab, _iconSlot);
        //         break;
        //     case "Planet":
        //         // Instantiate(_planetCorePrefab, _iconSlot);
        //         break;
        //     case "Flag":
        //         // Instantiate(_flagPrefab, _iconSlot);
        //         break;
        // }
    }

    // ========================================================================================
    // Account Functions
    // ========================================================================================
    
    public void LoadPreset()
    {
        _presetButton.SetActive(false);
        _appManager.LoadPreset();
        InspectAccount();
        InspectSelection();
        _appManager.LoadObjects();
    }

    public void SwitchAccount()
    {
        _appManager.SwitchAccount();
        InspectAccount();
        InspectSelection();
        Reload();
    }

    public void Reload()
    {
        _appManager.LoadObjects();
        InspectAccount();
        InspectSelection();
    }

    public void Unload()
    {
        _appManager.Unload();
        InspectAccount();
        InspectSelection();

        // _accountLoginPanel.SetActive(true);
        // _accountPanel.SetActive(false);

        _debugTextMeshes[0].text = "unloaded";
        _debugTextMeshes[1].text = "[...]";
        _debugTextMeshes[2].text = "[...]";
    }

    // calls Select() on the next child object in the list
    public void NextBody()
    {
        StellarObjectView currentView = _appManager.GetCurrentView();
        currentView.SelectNext();
        _appManager.UpdateSelection(currentView.GetSelectedCore().Id);
        InspectSelection();
    }

    public void PrevBody()
    {
        StellarObjectView currentView = _appManager.GetCurrentView();
        currentView.SelectPrev();
        _appManager.UpdateSelection(currentView.GetSelectedCore().Id);
        InspectSelection();
    }

    public void ZoomIn()
    {
        StellarObjectView currentView = _appManager.GetCurrentView();
        StellarObjectCore core = currentView.GetSelectedCore();
        if (core.IsChild)
        {
            _appManager.SetCurrentView(core.Id);
            Reload();
        }
    }

    public void ZoomOut()
    {
        int currentViewId = _appManager.GetCurrentView().Id;
        IStellarBody currentBody = _appManager.GetBody(currentViewId);
        if (currentBody.type != "Galaxy")
        {
            _appManager.SetCurrentView(currentBody.parentId);
            Reload();
        }
    }

    // ========================================================================================
    // Not yet implemented
    // ========================================================================================

    public void CreateAccount()
    {
        Account account = _appManager.CreateAccount("Tony Hawk");
        _debugTextMeshes[0].text = "Created account: " + account.username;
        _debugTextMeshes[1].text = "account id: " + account.id.ToString();
        _debugTextMeshes[2].text = "galaxy id: " + account.galaxyId.ToString();
    }

    public void CreateChild()
    {
        _appManager.CreateChild();
        InspectSelection();
    }


    public void RemoveSelection()
    {
        _appManager.RemoveSelection();
        InspectSelection();
    }







    // ========================================================================================
    // General functions
    // ========================================================================================
    public void SetPosition()
    {
        _appManager.SetPosition();
    }




    public void LoadAccount()
    {
        _appManager.LoadAccount("Tony Hawk");
        InspectAccount();
        InspectSelection();
    }

    // public void SetInspectedObjectName()
    // {
    //     string name = _stellarBodyManager.currentBody.customName;
    //     _textMeshes[0].text = name;
    // }

    // public void SetInspectedObjectId()
    // {
    //     string id = _stellarBodyManager.currentBody.id.ToString();
    //     _textMeshes[1].text = id;
    // }

    // public void SetUsername()
    // {
    //     string username = _accountManager.LoadAccount(_accountManager.currentAccountId).username;
    //     _textMeshes[2].text = username;
    // }

    // public void SetAccountId()
    // {
    //     string accountId = _accountManager.currentAccountId.ToString();
    //     _textMeshes[3].text = accountId;
    // }
}


