using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour, IDataPersistance
{
    private int lastBossId = 0;


    public void SpawnBoss()
    {
        switch (lastBossId)
        {
            case 0:
                // spawn boss 0
                BossKilled();
                break;
            case 1:
                // spawn boss 1
                BossKilled();
                break;
            case 2:
                // spawn boss 2
                BossKilled();
                break;
            case 3:
                // spawn boss 3
                BossKilled();
                break;
        }
    }


    public void BossKilled()
    {
        lastBossId++;

        GetComponent<MapCompletion>().ProgressToNextMap();
    }


    public void LoadData(GameData data)
    {
        this.lastBossId = data.lastBossId;
    }

    public void SaveData(ref GameData data)
    {
        data.lastBossId = this.lastBossId;
    }
}
