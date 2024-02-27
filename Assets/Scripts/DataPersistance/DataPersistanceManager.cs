using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
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
    }

    private void Start() 
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

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
