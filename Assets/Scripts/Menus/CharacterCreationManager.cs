using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreationManager : MonoBehaviour, IDataPersistance
{
    private int slectedCharacter;

    private bool[] isFirstBoot = new bool[4];

    private bool[] charExists = new bool[4];

    // TODO: new character creation
    public void CreateCharacter()
    {
        // check if character exists
        if (charExists[slectedCharacter])
        {
            // TODO: show warning
            return;
        }
        else
        {
            // create character
            charExists[slectedCharacter] = true;
            isFirstBoot[slectedCharacter] = true;
        }

        // save the data
        transform.GetComponent<DataPersistanceManager>().SaveGame();

        // switch to pre game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("PreGame");
    }

    public void Back()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterSelection");
    }

    // data persistance
    public void LoadData(GameData data)
    {
        this.slectedCharacter = data.selectedCharacter;
        this.isFirstBoot = data.isFirstBoot;
        this.charExists = data.charExists;
    }
    public void SaveData(ref GameData data)
    {
        data.selectedCharacter = this.slectedCharacter;
        data.isFirstBoot = this.isFirstBoot;
        data.charExists = this.charExists;
    }
}
