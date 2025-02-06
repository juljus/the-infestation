using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCompletion : MonoBehaviour, IDataPersistance
{
    // HACK: probably for deletion
        // [SerializeField] private TMPro.TMP_Text killCompletionCounter;
        // [SerializeField] private UnityEngine.UI.Image killCompletionBar;
        // [SerializeField] private GameObject structureCompletionBar;

    [SerializeField] private int killsToComplete;
    private int structuresToClear = 2;


    private int currentKills = 0;
    private int currentStructures = 0;


    private void Start()
    {
        // HACK: probably for deletion
            // killCompletionCounter.text = currentKills + "/" + killsToComplete;
            // killCompletionBar.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = (float)currentKills / killsToComplete;

            // if (currentStructures == 0)
            // {
            //     structureCompletionBar.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            //     structureCompletionBar.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            // }
            // else if (currentStructures == 1)
            // {
            //     structureCompletionBar.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            //     structureCompletionBar.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            // }
            // else if (currentStructures == 2)
            // {
            //     structureCompletionBar.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            //     structureCompletionBar.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            // }
    }


    public void AddKill()
    {
        currentKills++;

        // HACK: probably for deletion
            // if (currentKills > killsToComplete)
            // {
            //     currentKills = killsToComplete;
            // }
            // else
            // {
            //     killCompletionCounter.text = currentKills + "/" + killsToComplete;
            //     killCompletionBar.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = (float)currentKills / killsToComplete;
            // }
    }
    
    // HACK: probably for deletion
        // public void AddStructure()
        // {
        //     currentStructures++;

        //     if (currentStructures > structuresToClear)
        //     {
        //         currentStructures = structuresToClear;
        //     }
        //     else
        //     {
        //         if (currentStructures == 1)
        //         {
        //             structureCompletionBar.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        //             structureCompletionBar.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        //         }
        //         else if (currentStructures == 2)
        //         {
        //             structureCompletionBar.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        //             structureCompletionBar.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
        //         }
        //     }
        // }



    //! Data Persistance
    // TODO: implement InGameSave in the form of campfires and then move the needed saves to ingamesave instead of save

    public void InGameSave(ref GameData data)
    {
    }
    
    public void LoadData(GameData data)
    {
        int selectedCharacter = data.selectedChar;

        this.currentKills = data.charKills[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
        int selectedCharacter = data.selectedChar;

        data.charKills[selectedCharacter] = this.currentKills;
    }
}
