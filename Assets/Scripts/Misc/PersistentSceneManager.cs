using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSceneManager : MonoBehaviour
{
    public static PersistentSceneManager instance;

    [SerializeField] private Canvas loadingScreen;
    [SerializeField] private GameObject loadingCamera;

    private void Awake()
    {
        instance = this;

        SceneManager.LoadSceneAsync("CharacterSelection", LoadSceneMode.Additive);
    }

    public void LoadGameScene()
    {
        SceneManager.UnloadSceneAsync("StoryStart");
        SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
    }

    public void LoadScene(string startScene, string tarScene)
    {
        // display loading screen and camera
        loadingCamera.SetActive(true);
        loadingScreen.enabled = true;



        // unload start scene and load target scene
        SceneManager.UnloadSceneAsync(startScene);
        AsyncOperation loadingScene = SceneManager.LoadSceneAsync(tarScene, LoadSceneMode.Additive);
        loadingScene.allowSceneActivation = false;

        // start loading coroutine
        StartCoroutine(LoadingScene(loadingScene));
    }

    private IEnumerator LoadingScene(AsyncOperation loadingScene)
    {
        while (loadingScene.progress < 0.9f)
        {
            yield return null;
        }

        // hide loading screen and camera
        loadingCamera.SetActive(false);
        loadingScreen.enabled = false;

        loadingScene.allowSceneActivation = true;
    }
}
