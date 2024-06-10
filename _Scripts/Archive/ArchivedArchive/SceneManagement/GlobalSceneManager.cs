using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSceneManager : MonoBehaviour
{
    [Header("Scenes to Load")]
    [SerializeField] private SceneField _galaxyScene;
    [SerializeField] private SceneField _starScene;
    [SerializeField] private SceneField _planetScene;

    private SceneField _newScene;
    private SceneField _currentScene;

    private bool isSceneLoaded = false;
    // private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();

    void Awake()
    {
        _newScene = _galaxyScene;
    }

    public void SetNewScene(string scene)
    {
        switch (scene)
        {
            case "GalaxyScene":
                _newScene = _galaxyScene;
                break;
            case "StarScene":
                _newScene = _starScene;
                break;
            case "PlanetScene":
                _newScene = _planetScene;
                break;
        }
    }
    
    public void LoadScene()
    {
        UnloadScene();
        SceneManager.LoadSceneAsync(_newScene, LoadSceneMode.Additive);
        _currentScene = _newScene;
        isSceneLoaded = true;
    }

    private void UnloadScene()
    {
        if (isSceneLoaded)
        {
            SceneManager.UnloadSceneAsync(_currentScene);
        }
        isSceneLoaded = false;
    }
    
}
