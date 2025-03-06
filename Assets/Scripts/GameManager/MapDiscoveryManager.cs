using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapDiscoveryManager : MonoBehaviour, IDataPersistance
{
    private List<float> discoverableTilesX = new List<float>();
    private List<float> discoverableTilesY = new List<float>();

    [SerializeField] private GameObject tilesParent;
    [SerializeField] private GameObject discoverableTile;

    [SerializeField] private int chunkSize = 100;

    public static bool gameStarting = true;

    private IEnumerator TilesToList()
    {
        // empty lists
        discoverableTilesX.Clear();
        discoverableTilesY.Clear();

        // loop through all tiles and put them into the list
        int count = 0;
        foreach (Transform child in tilesParent.transform)
        {
            discoverableTilesX.Add(child.position.x);
            discoverableTilesY.Add(child.position.y);
            
            print("game still starting");

            count++;

            if (count % (chunkSize * 6) == 0)
            {
                yield return null;
            }
        }

        if (gameStarting)
        {
            print("SET GAME STARTING TO FALSE");
            gameStarting = false;
            Time.timeScale = 1;
        }

        yield return null;
    }

    private void TilesToListLegacy()
    {
        // empty lists
        discoverableTilesX.Clear();
        discoverableTilesY.Clear();

        // loop through all tiles and put them into the list
        foreach (Transform child in tilesParent.transform)
        {
            discoverableTilesX.Add(child.position.x);
            discoverableTilesY.Add(child.position.y);
        }
    }

    private void ListToTilesLegacy()
    {
        // delete all tiles on map and then replace them from the list
        foreach (Transform child in tilesParent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < discoverableTilesX.Count; i++)
        {
            GameObject tile = Instantiate(discoverableTile, new Vector3(discoverableTilesX[i], discoverableTilesY[i], 0), Quaternion.identity) as GameObject;
            tile.transform.SetParent(tilesParent.transform);
        }

        if (gameStarting)
        {
            gameStarting = false;
            Time.timeScale = 1;
        }
    }

    private IEnumerator ListToTiles()
    {
        // delete all tiles on map and then replace them from the list
        int count = 0;
        foreach (Transform child in tilesParent.transform)
        {
            print("destroying the tiles on map");

            Destroy(child.gameObject);

            count++;

            if (count % (chunkSize * 6) == 0)
            {
                yield return null;
            }
        }

        for (int i = 0; i < discoverableTilesX.Count; i++)
        {
            print("game still starting");

            GameObject tile = Instantiate(discoverableTile, new Vector3(discoverableTilesX[i], discoverableTilesY[i], 0), Quaternion.identity) as GameObject;
            tile.transform.SetParent(tilesParent.transform);

            if (i % chunkSize == 0 && i != 0)
            {
                yield return null;
            }
        }

        if (gameStarting)
        {
            print("SET GAME STARTING TO FALSE");
            gameStarting = false;
            Time.timeScale = 1;
        }

        yield return null;
    }

    //! IDataPersistance
    public void LoadData(GameData data)
    {
        print("loading data into map discovery manager");
        Time.timeScale = 0;

        int selectedChar = data.selectedChar;

        discoverableTilesX = data.discoverableTilesX[selectedChar];
        discoverableTilesY = data.discoverableTilesY[selectedChar];

        if (discoverableTilesX.Count == 0)
        {
            print("starting tile to list");
            StartCoroutine(TilesToList());
            // TilesToListLegacy();
        }
        else
        {
            print("starting list to tile");
            StartCoroutine(ListToTiles());
            // ListToTilesLegacy();
        }
    }

    public void SaveData(ref GameData data)
    {
    }

    public void InGameSave(ref GameData data)
    {
        print("saving data from map discovery manager");
        print("starting tile to list");
        StartCoroutine(TilesToList());
        // TilesToListLegacy();

        int selectedChar = data.selectedChar;

        data.discoverableTilesX[selectedChar] = discoverableTilesX;
        data.discoverableTilesY[selectedChar] = discoverableTilesY;
    }
}
