using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private GameObject[] characterCreationButtons = new GameObject[4];
    [SerializeField] private GameObject[] characterIcons = new GameObject[4];

    private int[] slotCharTypes = new int[4];

    private int selectedCharacter;

    void Start()
    {
        // hide character creation buttons
        for (int i = 0; i < slotCharTypes.Length; i++)
        {
            if (slotCharTypes[i] != 0)
            {
                characterCreationButtons[i].SetActive(false);
            }
        }

        // hide character icons
        for (int i = 0; i < slotCharTypes.Length; i++)
        {
            switch (slotCharTypes[i])
            {
                case 0:
                    characterIcons[i].SetActive(false);
                    break;
                case 1:
                    characterIcons[i].transform.GetChild(1).gameObject.SetActive(false);
                    break;
                case 2:
                    characterIcons[i].transform.GetChild(0).gameObject.SetActive(false);
                    break;
                default:
                    Debug.LogError("Invalid character type");
                    break;
            }
        }
    }

    public void StartCharacterCreation(int charNum)
    {
        // set the character being created
        selectedCharacter = charNum;

        // save the data
        transform.GetComponent<DataPersistanceManager>().SaveGame();

        // switch to character creation scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterCreation");
    }

    public void SelectCharacter(int charNum)
    {
        // set the character being selected
        selectedCharacter = charNum;

        // save the data
        transform.GetComponent<DataPersistanceManager>().SaveGame();

        // switch to pre game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("PreGame");
    }
    
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    // data persistance
    public void LoadData(GameData data)
    {
        this.slotCharTypes = data.slotCharacterTypes;
        this.selectedCharacter = data.selectedCharacter;
    }

    public void SaveData(ref GameData data)
    {
        data.slotCharacterTypes = this.slotCharTypes;
        data.selectedCharacter = this.selectedCharacter;
    }
}
