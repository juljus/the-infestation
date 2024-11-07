using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGameManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private GameObject characterIcon;
    [SerializeField] private TMPro.TMP_Text characterLevelText;

    private int selectedCharacter;
    private int[] slotCharacterTypes;
    private int[] characterLevels;

    void Start()
    {
        if (slotCharacterTypes[selectedCharacter] == 1)
        {
            characterIcon.transform.GetChild(1).gameObject.SetActive(false);
        }
        else if (slotCharacterTypes[selectedCharacter] == 2)
        {
            characterIcon.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Invalid character type");
        }

        // display character level
        characterLevelText.text = "Level: " + characterLevels[selectedCharacter];
    }

    public void StartGame()
    {
        // load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void Back()
    {
        // load the character selection scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterSelection");
    }

    public void DeleteCharacter()
    {
        // delete the selected character
        slotCharacterTypes[selectedCharacter] = 0;

        // delete the selected characters data
        characterLevels[selectedCharacter] = 0;

        // save the data
        transform.GetComponent<DataPersistanceManager>().SaveGame();

        // load the character selection scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterSelection");
    }

    // data persistance
    public void LoadData(GameData data) {
        this.selectedCharacter = data.selectedCharacter;
        this.slotCharacterTypes = data.slotCharacterTypes;
        this.characterLevels = data.characterLevels;
    }
    public void SaveData(ref GameData data) {
        data.selectedCharacter = this.selectedCharacter;
        data.slotCharacterTypes = this.slotCharacterTypes;
        data.characterLevels = this.characterLevels;
    }
}
