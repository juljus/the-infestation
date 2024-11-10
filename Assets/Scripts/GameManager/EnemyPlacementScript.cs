using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyPlacementScript : MonoBehaviour, IDataPersistance
{
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();

    private List<float> enemyPositionsX = new List<float>();
    private List<float> enemyPositionsY = new List<float>();
    private List<int> enemyIDs = new List<int>();

    void Start()
    {
        if (enemyIDs.Count > 0)
        {
            PlaceEnemies();
        }
    }

    private void PlaceEnemies()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        int i = 0;
        foreach (int enemyID in enemyIDs)
        {
            print("starting enemy placement");
            GameObject enemy = Instantiate(enemyPrefabs[enemyID], new Vector3(enemyPositionsX[i], enemyPositionsY[i], 0), Quaternion.identity);
            enemy.name = enemyPrefabs[enemyID].name;
            enemy.transform.SetParent(GameObject.Find("Enemies").transform);
            print("Enemy placed: " + enemy.name);
            i++;
        }
    }

    public void LoadData(GameData data)
    {
        this.enemyIDs = data.enemyIDs;
        this.enemyPositionsX = data.enemyPositionsX;
        this.enemyPositionsY = data.enemyPositionsY;
    }

    public void SaveData(ref GameData data)
    {
        // data.characterLevels = this.characterLevels;

        GameObject[] enemiesOnMap = GameObject.FindGameObjectsWithTag("Enemy");

        enemyPositionsX.Clear();
        enemyPositionsY.Clear();
        enemyIDs.Clear();

        foreach (GameObject enemy in enemiesOnMap)
        {
            this.enemyPositionsX.Add(enemy.transform.position.x);
            this.enemyPositionsY.Add(enemy.transform.position.y);

            foreach (GameObject enemyPrefab in this.enemyPrefabs)
            {
                print("Enemy prefab name: " + enemyPrefab.name);
                if (enemyPrefab.name == enemy.name)
                {
                    enemyIDs.Add(this.enemyPrefabs.IndexOf(enemyPrefab));
                    print("ID added: " + this.enemyPrefabs.IndexOf(enemyPrefab));
                }
            }
        }

        print("Enemy positions X: " + enemyPositionsX.Count);
        print("Enemy positions Y: " + enemyPositionsY.Count);
        print("Enemy IDs: " + enemyIDs.Count);

        data.enemyPositionsX = enemyPositionsX;
        data.enemyPositionsY = enemyPositionsY;
        data.enemyIDs = enemyIDs;
    }
}
