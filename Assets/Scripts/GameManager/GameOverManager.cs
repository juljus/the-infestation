using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverManager : MonoBehaviour, IDataPersistance
{
    // TODO: death should put the player back to last save

    private bool charDead = false;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private UnityEngine.UI.Image blackScreen;

    private bool gameOver = false;

    void Start()
    {
        if (charDead)
        {
            GameOver();
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        PersistentSceneManager.instance.LoadSceneWithLoadingScreen("Game", "PreGame");
    }

    public void GameOver()
    {
        if (gameOver)
        {
            return;
        }

        gameOver = true;

        // stop the game
        Time.timeScale = 0;

        // start game over coroutine
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        // set active black screen
        blackScreen.color = new Color(0, 0, 0, 0);
        blackScreen.gameObject.SetActive(true);

        // fade in black screen
        float fadeTime = 2f;
        float elapsedTime = 0f;
        float startTime = Time.realtimeSinceStartup;

        while (elapsedTime < fadeTime)
        {
            elapsedTime = Time.realtimeSinceStartup - startTime;
            blackScreen.color = new Color(0, 0, 0, elapsedTime / fadeTime);
            yield return null;
        }
        
        // wait for 1 second
        startTime = Time.realtimeSinceStartup;
        float waitTime = 1f;

        while (Time.realtimeSinceStartup - startTime < waitTime)
        {
            yield return null;
        }

        // switch to pregame scene
        PersistentSceneManager.instance.LoadSceneWithLoadingScreen("Game", "PreGame");
    }

    //! IDataPersistance

    public void LoadData(GameData data)
    {
        int selectedChar = data.selectedChar;

        charDead = data.charDead[selectedChar];
    }

    public void SaveData(ref GameData data)
    {
        int selectedChar = data.selectedChar;

        data.charDead[selectedChar] = charDead;
    }

    public void InGameSave(ref GameData data)
    {
    }
}