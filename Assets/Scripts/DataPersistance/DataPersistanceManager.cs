using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    private string fileName = "GameData.yaml";
    private GameData gameData;
    private List<IDataPersistance> dataPersistanceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistanceManager instance { get; private set; }

    private void Awake() 
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one DataPersistanceManager in the scene");
        }
        instance = this;

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
        print("Game Loaded");
    }

    public void SetPlayerStats(PlayerScriptableObject playerScriptableObject)
    {
        // physical stats
        this.gameData.playerAttackDamage[this.gameData.selectedChar] = playerScriptableObject.attackDamage;
        this.gameData.playerAttackTime[this.gameData.selectedChar] = playerScriptableObject.attackTime;
        this.gameData.playerAttackRange[this.gameData.selectedChar] = playerScriptableObject.attackRange;
        this.gameData.playerAttackCooldown[this.gameData.selectedChar] = playerScriptableObject.attackCooldown;
        this.gameData.playerMovementSpeed[this.gameData.selectedChar] = playerScriptableObject.movementSpeed;
        this.gameData.playerHealth[this.gameData.selectedChar] = playerScriptableObject.health;

        this.gameData.charLevels[this.gameData.selectedChar] = 0;
        this.gameData.learnedSkills[this.gameData.selectedChar] = new bool[8];
        this.gameData.charKills[this.gameData.selectedChar] = 0;

        print("Player Stats Saved");

        dataHandler.Save(gameData);

        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    // save that automatically occurs
    public void SaveGame()
    {
        // pass data to other scripts so they can update it
        foreach (IDataPersistance dataPersistanceObject in this.dataPersistanceObjects)
        {
            dataPersistanceObject.SaveData(ref this.gameData);
        }

        // save the data to a file using the data handler
        dataHandler.Save(gameData);
    }

    // save that occurs only when called
    // TODO: implement InGameSave in the form of campfires and then move the needed saves to ingamesave instead of save
    public void InGameSave()
    {
        // pass data to other scripts so they can update it
        foreach (IDataPersistance dataPersistanceObject in this.dataPersistanceObjects)
        {
            dataPersistanceObject.InGameSave(ref this.gameData);
        }

        // save the data to a file using the data handler
        dataHandler.Save(gameData);
    }

    // load the game data
    public void LoadGame()
    {
        // load any saved data from a file using the data handler
        this.gameData = dataHandler.Load();

        // if no data can be loaded initialize to a new game
        if (this.gameData == null)
        {
            NewGame();
        }

        // push the loaded data to all other scripts
        foreach (IDataPersistance dataPersistanceObject in this.dataPersistanceObjects)
        {
            dataPersistanceObject.LoadData(this.gameData);
        }
    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }
}
