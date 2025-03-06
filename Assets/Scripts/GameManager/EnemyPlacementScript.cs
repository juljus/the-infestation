using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyPlacementScript : MonoBehaviour, IDataPersistance
{
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();

    private List<float>[] enemyPositionsX;
    private List<float>[] enemyPositionsY;
    private List<int>[] enemyIDs;
    
    [SerializeField] private GameObject enemyTilemap;

    private int selectedChar;
    

    void Start()
    {
        // HACK: if all enemies are killed, they are all placed again!!
        if (enemyIDs[selectedChar].Count > 0)
        {
            PlaceEnemies();
        }
    }

    public void PlaceEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        int i = 0;
        foreach (int enemyID in enemyIDs[selectedChar])
        {
            GameObject enemy = Instantiate(enemyPrefabs[enemyID], new Vector3(enemyPositionsX[selectedChar][i], enemyPositionsY[selectedChar][i], 0), Quaternion.identity);
            enemy.name = enemyPrefabs[enemyID].name;
            enemy.transform.SetParent(enemyTilemap.transform);
            i++;
        }
    }

    //! Data Persistance
    public void InGameSave(ref GameData data)
    {
        GameObject[] enemiesOnMap = GameObject.FindGameObjectsWithTag("Enemy");


        enemyPositionsX[selectedChar].Clear();
        enemyPositionsY[selectedChar].Clear();
        enemyIDs[selectedChar].Clear();

        foreach (GameObject enemy in enemiesOnMap)
        {
            this.enemyPositionsX[selectedChar].Add(enemy.transform.position.x);
            this.enemyPositionsY[selectedChar].Add(enemy.transform.position.y);

            foreach (GameObject enemyPrefab in this.enemyPrefabs)
            {
                if (enemyPrefab.name == enemy.name)
                {
                    enemyIDs[selectedChar].Add(this.enemyPrefabs.IndexOf(enemyPrefab));
                }
            }
        }

        data.enemyPositionsX = this.enemyPositionsX;
        data.enemyPositionsY = this.enemyPositionsY;
        data.enemyIDs = this.enemyIDs;
    }

    public void LoadData(GameData data)
    {        
        this.enemyIDs = data.enemyIDs;
        this.enemyPositionsX = data.enemyPositionsX;
        this.enemyPositionsY = data.enemyPositionsY;
        this.selectedChar = data.selectedChar;
    }

    public void SaveData(ref GameData data)
    {
    }
}
