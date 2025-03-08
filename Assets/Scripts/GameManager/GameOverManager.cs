using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverManager : MonoBehaviour, IDataPersistance
{
    private bool charDead = false;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private UnityEngine.UI.Image blackScreen;

    private bool gameOver = false;

    private float playerPosX;
    private float playerPosY;

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
        GameObject player = GameObject.Find("GameManager").GetComponent<PlayerManager>().GetPlayer;

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
        
        // wait for 0.5 second
        startTime = Time.realtimeSinceStartup;
        float waitTime = 0.5f;

        while (Time.realtimeSinceStartup - startTime < waitTime)
        {
            yield return null;
        }

        // trigger LoadData functions
        transform.GetComponent<DataPersistanceManager>().LoadGame();

        // replace all enemies
        transform.GetComponent<EnemyPlacementScript>().PlaceEnemies();

        // reset player position
        player.transform.position = new Vector3(playerPosX, playerPosY, 0);

        // reset player health / cooldowns
        player.GetComponent<PlayerHealth>().ResetHealth();

        PlayerSkillHolder playerSkillHolder = player.transform.GetComponent<PlayerSkillHolder>();
        playerSkillHolder.ResetAllSkills();

        PlayerAttack playerAttack = player.transform.GetComponent<PlayerAttack>();
        playerAttack.SetIsAttacking(false);

        // fade out black screen
        startTime = Time.realtimeSinceStartup;
        fadeTime = 0.5f;
        elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime = Time.realtimeSinceStartup - startTime;
            blackScreen.color = new Color(0, 0, 0, 1 - elapsedTime / fadeTime);
            yield return null;
        }

        // set black screen inactive
        blackScreen.gameObject.SetActive(false);

        // start the game
        gameOver = false;
        Time.timeScale = 1;
    }

    //! IDataPersistance

    public void LoadData(GameData data)
    {
        int selectedChar = data.selectedChar;

        charDead = data.charDead[selectedChar];

        float[] playerPos = data.charCoords[selectedChar];
        playerPosX = playerPos[0];
        playerPosY = playerPos[1];
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