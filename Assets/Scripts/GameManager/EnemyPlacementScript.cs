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

    private int selectedChar;
    

    void Start()
    {
        // BUG: if all enemies are killed, they are all placed again... bad!!
        if (enemyIDs[selectedChar].Count > 0)
        {
            print("PLACED ENEMIES");
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
        foreach (int enemyID in enemyIDs[selectedChar])
        {
            print("starting enemy placement");
            GameObject enemy = Instantiate(enemyPrefabs[enemyID], new Vector3(enemyPositionsX[selectedChar][i], enemyPositionsY[selectedChar][i], 0), Quaternion.identity);
            enemy.name = enemyPrefabs[enemyID].name;
            enemy.transform.SetParent(GameObject.Find("Enemies").transform);
            print("Enemy placed: " + enemy.name);
            i++;
        }
    }

    //! Data Persistance
    // TODO: implement InGameSave in the form of campfires and then move the needed saves to ingamesave instead of save
    public void InGameSave(ref GameData data)
    {
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
        GameObject[] enemiesOnMap = GameObject.FindGameObjectsWithTag("Enemy");

        print("Enemies on map: " + enemiesOnMap.Length);

        enemyPositionsX[selectedChar].Clear();
        enemyPositionsY[selectedChar].Clear();
        enemyIDs[selectedChar].Clear();

        foreach (GameObject enemy in enemiesOnMap)
        {
            print("asdf+");
            this.enemyPositionsX[selectedChar].Add(enemy.transform.position.x);
            this.enemyPositionsY[selectedChar].Add(enemy.transform.position.y);

            foreach (GameObject enemyPrefab in this.enemyPrefabs)
            {
                print("Enemy prefab name: " + enemyPrefab.name);
                if (enemyPrefab.name == enemy.name)
                {
                    enemyIDs[selectedChar].Add(this.enemyPrefabs.IndexOf(enemyPrefab));
                    print("ID added: " + this.enemyPrefabs.IndexOf(enemyPrefab));
                }
            }
        }

        print("Enemy positions X: " + this.enemyPositionsX[selectedChar].Count);
        print("Enemy positions Y: " + this.enemyPositionsY[selectedChar].Count);
        print("Enemy IDs: " + this.enemyIDs[selectedChar].Count);

        data.enemyPositionsX = this.enemyPositionsX;
        data.enemyPositionsY = this.enemyPositionsY;
        data.enemyIDs = this.enemyIDs;
    }
}
