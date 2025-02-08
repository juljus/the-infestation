using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreGameManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private GameObject characterIcon;
    [SerializeField] private TMPro.TMP_Text characterLevelText;
    [SerializeField] private TMPro.TMP_Text characterNameText;
    [SerializeField] private TMPro.TMP_Text characterKillsText;
    [SerializeField] private GameObject deleteCharacterPopup;

    private int selectedCharacter;
    private int[] characterLevels;
    private bool[] charExists;
    private string[] charNames;
    private int[] charKills;

    void Start()
    {
        // display character level, kills and name
        characterLevelText.text = "Level: " + characterLevels[selectedCharacter];
        characterNameText.text = charNames[selectedCharacter];
        characterKillsText.text = "Kills: " + charKills[selectedCharacter];
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

    public void DeleteCharacterPopupOn()
    {
        // display the delete character popup
        deleteCharacterPopup.SetActive(true);
    }

    public void DeleteCharacterPopupOff()
    {
        // hide the delete character popup
        deleteCharacterPopup.SetActive(false);
    }

    public void DeleteCharacter()
    {
        charExists[selectedCharacter] = false;

        // save the data
        transform.GetComponent<DataPersistanceManager>().SaveGame();

        // load the character selection scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterSelection");
    }

    //! data persistance
    public void InGameSave(ref GameData data) {
    }

    public void LoadData(GameData data) {
        this.selectedCharacter = data.selectedChar;
        this.characterLevels = data.charLevels;
        this.charExists = data.charExists;
        this.charNames = data.charNames;
        this.charKills = data.charKills;
    }
    public void SaveData(ref GameData data) {
        data.charExists = this.charExists;
    }
}
