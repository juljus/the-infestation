using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCompletion : MonoBehaviour, IDataPersistance
{
    [SerializeField] private TMPro.TMP_Text killCompletionCounter;
    [SerializeField] private UnityEngine.UI.Image killCompletionBar;
    [SerializeField] private GameObject structureCompletionBar;

    [SerializeField] private int killsToComplete;
    private int structuresToClear = 2;


    private int currentKills = 0;
    private int currentStructures = 0;


    private void Start()
    {
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




    
    public void LoadData(GameData data)
    {
        int selectedCharacter = data.selectedCharacter;

        this.currentKills = data.currentKills[selectedCharacter];
        this.currentStructures = data.currentStructures[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
        int selectedCharacter = data.selectedCharacter;

        data.currentKills[selectedCharacter] = this.currentKills;
        data.currentStructures[selectedCharacter] = this.currentStructures;
    }
}
