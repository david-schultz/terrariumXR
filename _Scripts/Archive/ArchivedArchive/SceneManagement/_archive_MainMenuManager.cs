using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// from https://www.youtube.com/watch?v=6-0zD9Xyu5c&list=TLPQMDEwNDIwMjSY-_nq1L4Eug&index=2

public class ArchiveMainMenuManager : MonoBehaviour
{
    [Header("Main Menu Objects")]
    [SerializeField] private GameObject _loadingBarObject;
    [SerializeField] private Image _loadingBar;
    [SerializeField] private GameObject[] _objectsToHide;

    [Header("Scenes to Load")]
    [SerializeField] private SceneField _persistentScene;
    [SerializeField] private SceneField _galaxyScene;
    [SerializeField] private SceneField _starScene;
    [SerializeField] private SceneField _planetScene;

    private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();

    private void Awake()
    {
        _loadingBarObject.SetActive(false);
    }

    public void StartGame()
    {
        HideMenu();

        _loadingBarObject.SetActive(true);

        _scenesToLoad.Add(SceneManager.LoadSceneAsync(_persistentScene, LoadSceneMode.Single));
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(_galaxyScene, LoadSceneMode.Additive));
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(_starScene, LoadSceneMode.Additive));
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(_planetScene, LoadSceneMode.Additive));

        StartCoroutine(ProgressLoadingBar());
    }

    private void HideMenu()
    {
        for (int i = 0; i < _objectsToHide.Length; i++)
        {
            _objectsToHide[i].SetActive(false);
        }
    }

    private IEnumerator ProgressLoadingBar()
    {
        float loadProgress = 0f;
        for (int i = 0; i < _scenesToLoad.Count; i++)
        {
            while (!_scenesToLoad[i].isDone)
            {
                loadProgress += _scenesToLoad[i].progress;
                _loadingBarObject.GetComponent<Slider>().value = loadProgress / _scenesToLoad.Count;
                yield return null;
            }
        }
    }
}
