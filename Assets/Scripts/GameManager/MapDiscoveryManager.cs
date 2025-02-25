using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDiscoveryManager : MonoBehaviour, IDataPersistance
{
    private List<float> discoverableTilesX = new List<float>();
    private List<float> discoverableTilesY = new List<float>();

    [SerializeField] private GameObject tilesParent;
    [SerializeField] private GameObject discoverableTile;

    private void TilesToList()
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

    private void ListToTiles()
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
    }

    //! IDataPersistance
    public void LoadData(GameData data)
    {
        int selectedChar = data.selectedChar;

        discoverableTilesX = data.discoverableTilesX[selectedChar];
        discoverableTilesY = data.discoverableTilesY[selectedChar];

        if (discoverableTilesX.Count == 0)
        {
            print("starting sequence");
            TilesToList();
        }
        else
        {
            print("replacing tiles");
            ListToTiles();
        }
    }

    public void SaveData(ref GameData data)
    {
    }

    public void InGameSave(ref GameData data)
    {
        TilesToList();

        int selectedChar = data.selectedChar;

        data.discoverableTilesX[selectedChar] = discoverableTilesX;
        data.discoverableTilesY[selectedChar] = discoverableTilesY;
    }
}
