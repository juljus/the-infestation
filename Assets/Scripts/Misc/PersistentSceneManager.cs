using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSceneManager : MonoBehaviour
{
    public static PersistentSceneManager instance;

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
        SceneManager.UnloadSceneAsync(startScene);
        SceneManager.LoadSceneAsync(tarScene, LoadSceneMode.Additive);
    }
}
