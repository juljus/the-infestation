using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreationManager : MonoBehaviour, IDataPersistance
{
    private int slectedCharacter;

    private int[] slotCharacterTypes = new int[4];
    public void CreateCharacter(int charType)
    {
        // set the character type
        switch (slectedCharacter)
        {
            case 0:
                slotCharacterTypes[slectedCharacter] = charType;
                break;
            case 1:
                slotCharacterTypes[slectedCharacter] = charType;
                break;
            case 2:
                slotCharacterTypes[slectedCharacter] = charType;
                break;
            case 3:
                slotCharacterTypes[slectedCharacter] = charType;
                break;
            default:
                Debug.LogError("Invalid character number");
                break;
        }

        // save the data
        transform.GetComponent<DataPersistanceManager>().SaveGame();

        // switch to character selection scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterSelection");
    }

    // data persistance
    public void LoadData(GameData data)
    {
        this.slotCharacterTypes = data.slotCharacterTypes;
        this.slectedCharacter = data.selectedCharacter;
    }
    public void SaveData(ref GameData data)
    {
        data.slotCharacterTypes = this.slotCharacterTypes;
        data.selectedCharacter = this.slectedCharacter;
    }
}
