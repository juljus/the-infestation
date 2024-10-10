using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCompletion : MonoBehaviour, IDataPersistance
{
    [SerializeField] private TMPro.TMP_Text mapText;
    [SerializeField] private TMPro.TMP_Text killCompletionCounter;
    [SerializeField] private UnityEngine.UI.Image killCompletionBar;
    [SerializeField] private GameObject structureCompletionBar;

    [SerializeField] private int numOfMaps;
    [SerializeField] private int killsToComplete;
    private int structuresToClear = 2;


    private int currentKills = 0;
    private int currentStructures = 0;
    private int currentMapId = 0;


    private void Start()
    {
        mapText.text = "Map " + (currentMapId) + "/" + numOfMaps;
        killCompletionCounter.text = currentKills + "/" + killsToComplete;
        killCompletionBar.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = (float)currentKills / killsToComplete;

        if (currentStructures == 0)
        {
            structureCompletionBar.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            structureCompletionBar.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        }
        else if (currentStructures == 1)
        {
            structureCompletionBar.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            structureCompletionBar.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        }
        else if (currentStructures == 2)
        {
            structureCompletionBar.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            structureCompletionBar.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        }
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
    

    public void AddStructure()
    {
        currentStructures++;

        if (currentStructures > structuresToClear)
        {
            currentStructures = structuresToClear;
        }
        else
        {
            if (currentStructures == 1)
            {
                structureCompletionBar.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                structureCompletionBar.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            }
            else if (currentStructures == 2)
            {
                structureCompletionBar.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                structureCompletionBar.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            }
        }
    }


    public void ProgressToNextMap()
    {
        currentMapId++;
        currentKills = 0;
        currentStructures = 0;

        if (currentMapId > numOfMaps)
        {
            // end game
        }
        else
        {
            mapText.text = "Map " + (currentMapId) + "/" + numOfMaps;
            killCompletionCounter.text = currentKills + "/" + killsToComplete;
            killCompletionBar.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = (float)currentKills / killsToComplete;
            structureCompletionBar.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            structureCompletionBar.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ResetToFistMap()
    {
        currentMapId = 0;
        currentKills = 0;
        currentStructures = 0;

        mapText.text = "Map " + (currentMapId) + "/" + numOfMaps;
        killCompletionCounter.text = currentKills + "/" + killsToComplete;
        killCompletionBar.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = (float)currentKills / killsToComplete;
        structureCompletionBar.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        structureCompletionBar.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
    }

    
    public void LoadData(GameData data)
    {
        int selectedCharacter = data.selectedCharacter;

        this.currentKills = data.currentKills[selectedCharacter];
        this.currentMapId = data.currentMap[selectedCharacter];
        this.currentStructures = data.currentStructures[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
        int selectedCharacter = data.selectedCharacter;

        data.currentKills[selectedCharacter] = this.currentKills;
        data.currentMap[selectedCharacter] = this.currentMapId;
        data.currentStructures[selectedCharacter] = this.currentStructures;
    }
}
