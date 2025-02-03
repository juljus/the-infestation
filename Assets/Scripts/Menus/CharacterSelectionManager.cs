using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// TODO: rework whole scene

public class CharacterSelectionManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] private GameObject[] characterCreationButtons = new GameObject[4];
    [SerializeField] private GameObject[] characterIcons = new GameObject[4];

    private int selectedCharacter;

    void Start()
    {
        // TODO: hide character creation buttons

        // TODO: hide character icons
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
        this.selectedCharacter = data.selectedCharacter;
    }

    public void SaveData(ref GameData data)
    {
        data.selectedCharacter = this.selectedCharacter;
    }
}
