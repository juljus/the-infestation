using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverManager : MonoBehaviour, IDataPersistance
{
    private bool charDead = false;

    [SerializeField] private GameObject gameOverScreen;

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
        UnityEngine.SceneManagement.SceneManager.LoadScene("PreGame");
    }

    public void GameOver()
    {
        // stop the game
        Time.timeScale = 0;

        // set the char as dead
        charDead = true;

        // trigger saves
        transform.GetComponent<DataPersistanceManager>().SaveGame();
        transform.GetComponent<DataPersistanceManager>().InGameSave();
        
        // show the game over screen
        gameOverScreen.SetActive(true);
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