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

    public static bool gameStarting = false;

    private IEnumerator TilesToList()
    {
        // empty lists
        discoverableTilesX.Clear();
        discoverableTilesY.Clear();

        // loop through all tiles and put them into the list
        foreach (Transform child in tilesParent.transform)
        {
            discoverableTilesX.Add(child.position.x);
            discoverableTilesY.Add(child.position.y);
            
            print("game still starting");

            yield return null;
        }

        if (gameStarting)
        {
            gameStarting = false;
            Time.timeScale = 1;
        }
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
        foreach (Transform child in tilesParent.transform)
        {
            Destroy(child.gameObject);

            yield return null;
        }

        for (int i = 0; i < discoverableTilesX.Count; i++)
        {
            GameObject tile = Instantiate(discoverableTile, new Vector3(discoverableTilesX[i], discoverableTilesY[i], 0), Quaternion.identity) as GameObject;
            tile.transform.SetParent(tilesParent.transform);

            print("game still starting");

            yield return null;
        }

        if (gameStarting)
        {
            gameStarting = false;
            Time.timeScale = 1;
        }
    }

    //! IDataPersistance
    public void LoadData(GameData data)
    {
        gameStarting = true;
        Time.timeScale = 0;

        int selectedChar = data.selectedChar;

        discoverableTilesX = data.discoverableTilesX[selectedChar];
        discoverableTilesY = data.discoverableTilesY[selectedChar];

        if (discoverableTilesX.Count == 0)
        {
            print("starting sequence");
            // StartCoroutine(TilesToList());
            TilesToListLegacy();
        }
        else
        {
            print("replacing tiles");
            // StartCoroutine(ListToTiles());
            ListToTilesLegacy();
        }
    }

    public void SaveData(ref GameData data)
    {
    }

    public void InGameSave(ref GameData data)
    {
        // StartCoroutine(TilesToList());
        TilesToListLegacy();

        int selectedChar = data.selectedChar;

        data.discoverableTilesX[selectedChar] = discoverableTilesX;
        data.discoverableTilesY[selectedChar] = discoverableTilesY;
    }
}
