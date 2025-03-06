using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private GameObject[] characterIcons = new GameObject[4];
    [SerializeField] private TMPro.TMP_Text[] characterLevelText = new TMPro.TMP_Text[4];
    [SerializeField] private TMPro.TMP_Text[] characterNameText = new TMPro.TMP_Text[4];
    [SerializeField] private GameObject[] deadText;

    private int selectedCharacter;
    private bool[] charExists = new bool[4];
    private string[] charNames = new string[4];
    private int[] charLevels = new int[4];
    private bool[] charDead = new bool[4];

    void Start()
    {
        // hide character icons, level and name
        for (int i = 0; i < 4; i++)
        {
            if (charExists[i])
            {
                characterIcons[i].SetActive(true);
                characterNameText[i].gameObject.SetActive(true);
                characterLevelText[i].gameObject.SetActive(true);

                characterNameText[i].text = charNames[i];
                characterLevelText[i].text = "Level: " + charLevels[i];
            }
            else
            {
                characterIcons[i].SetActive(false);
                characterNameText[i].gameObject.SetActive(false);
                characterLevelText[i].gameObject.SetActive(false);
            }
        }

        // show dead text
        for (int i = 0; i < 4; i++)
        {
            if (charExists[i] && charDead[i])
            {
                deadText[i].SetActive(true);
            }
        }
    }

    public void SelectSlot(int charNum)
    {
        // check if char already exists
        if (charExists[charNum])
        {
            // select the character
            SelectCharacter(charNum);
            return;
        }

        // set the character being created
        selectedCharacter = charNum;

        // save the data
        transform.GetComponent<DataPersistanceManager>().SaveGame();

        // switch to character creation scene
        PersistentSceneManager.instance.LoadSceneWithoutLoadingScreen("CharacterSelection", "CharacterCreation");
    }

    public void SelectCharacter(int charNum)
    {
        // set the character being selected
        selectedCharacter = charNum;

        // save the data
        transform.GetComponent<DataPersistanceManager>().SaveGame();

        // switch to pre game scene
        PersistentSceneManager.instance.LoadSceneWithoutLoadingScreen("CharacterSelection", "PreGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    //! data persistance
    public void InGameSave(ref GameData data)
    {
    }

    public void LoadData(GameData data)
    {
        this.selectedCharacter = data.selectedChar;
        this.charExists = data.charExists;
        this.charNames = data.charNames;
        this.charLevels = data.charLevels;
        this.charDead = data.charDead;
    }

    public void SaveData(ref GameData data)
    {
        data.selectedChar = this.selectedCharacter;
    }
}
