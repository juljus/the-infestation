using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCompletion : MonoBehaviour, IDataPersistance
{
    [SerializeField] private TMPro.TMP_Text mapText;
    [SerializeField] private TMPro.TMP_Text killCompletionCounter;
    [SerializeField] private UnityEngine.UI.Image killCompletionBar;

    [SerializeField] private int numOfMaps;
    [SerializeField] private int killsToComplete;


    private int currentKills = 0;
    private int currentMap = 0;


    private void Start()
    {
        mapText.text = "Map " + (currentMap) + "/" + numOfMaps;
        killCompletionCounter.text = currentKills + "/" + killsToComplete;
        killCompletionBar.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = (float)currentKills / killsToComplete;
    }


    public void AddKill()
    {
        currentKills++;

        if (currentKills > killsToComplete)
        {
            currentKills = killsToComplete;
        }
        else
        {
            killCompletionCounter.text = currentKills + "/" + killsToComplete;
            killCompletionBar.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = (float)currentKills / killsToComplete;
        }
    }

    
    public void LoadData(GameData data)
    {
        this.currentKills = data.currentKills;
        this.currentMap = data.currentMap;
    }

    public void SaveData(ref GameData data)
    {
        data.currentKills = this.currentKills;
        data.currentMap = this.currentMap;
    }
}
