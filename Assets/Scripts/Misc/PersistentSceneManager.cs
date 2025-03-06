using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSceneManager : MonoBehaviour
{
    public static PersistentSceneManager instance;

    [SerializeField] private Canvas loadingScreen;
    [SerializeField] private Camera loadingCamera;

    private void Awake()
    {
        instance = this;

        SceneManager.LoadSceneAsync("CharacterSelection", LoadSceneMode.Additive);
    }

    public void LoadGameScene(string startScene)
    {
        // display loading screen and camera
        loadingCamera.enabled = true;
        loadingScreen.enabled = true;

        // unload start scene and load target scene
        SceneManager.UnloadSceneAsync(startScene);
        AsyncOperation loadingScene = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        loadingScene.allowSceneActivation = false;

        // start loading coroutine
        StartCoroutine(LoadingScene(loadingScene));
    }

    public void LoadSceneWithoutLoadingScreen(string startScene, string tarScene)
    {
        // unload start scene and load target scene
        SceneManager.UnloadSceneAsync(startScene);
        SceneManager.LoadScene(tarScene, LoadSceneMode.Additive);
    }

    private IEnumerator LoadingScene(AsyncOperation loadingScene)
    {
        while (loadingScene.progress < 0.9f)
        {
            yield return null;
        }

        // wait for a bit
        yield return new WaitForSeconds(2f);

        loadingScene.allowSceneActivation = true;

        // set the loading screen and camera to be active again (also the star coroutine)
        loadingCamera.enabled = true;
        loadingScreen.enabled = true;
        StartCoroutine(gameObject.GetComponent<StarPopupScript>().CycleStars());


        while (MapDiscoveryManager.gameStarting)
        {
            yield return null;
        }

        print("NOW HIDING LOADING SCREEN");

        // hide loading screen and camera
        loadingCamera.enabled = false;
        loadingScreen.enabled = false;
    }
}
