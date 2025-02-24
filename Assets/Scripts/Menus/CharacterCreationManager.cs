using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreationManager : MonoBehaviour, IDataPersistance
{
    private int slectedCharacter;

    private bool[] charExists = new bool[4];
    private string[] charNames = new string[4];

    [SerializeField] private PlayerScriptableObject playerScriptableObject;
    [SerializeField] private TMPro.TMP_Text characterNameText;
    [SerializeField] private TMPro.TMP_Text nameInfoText;

    public void CreateCharacter()
    {
        // delete trailing whitespaces from the name
        characterNameText.text = characterNameText.text.Trim();

        // check if character already exists
        if (charExists[slectedCharacter])
        {
            Debug.LogError("Character already exists");
            return;
        }

        // check if name is of acceptable length (3 - 10)
        if (characterNameText.text.Length < 4)
        {
            Debug.Log("Name too short: " + characterNameText.text);
            StartCoroutine(FlashRed(nameInfoText));
            return;
        }
        else if (characterNameText.text.Length > 11)
        {
            Debug.Log("Name too long: " + characterNameText.text);
            StartCoroutine(FlashRed(nameInfoText));
            return;
        }

        // set all the player stats
        GameObject.Find("GameManager").GetComponent<DataPersistanceManager>().SetPlayerStats(playerScriptableObject);

        // set the name
        Debug.Log("Setting name: " + characterNameText.text);
        charNames[slectedCharacter] = characterNameText.text;

        // set as existing
        charExists[slectedCharacter] = true;

        // save the data
        transform.GetComponent<DataPersistanceManager>().SaveGame();

        // switch to pre game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("PreGame");
    }

    public void Back()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterSelection");
    }

    private IEnumerator FlashRed(TMPro.TMP_Text text)
    {
        print("flashing red");
        text.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        text.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        text.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        text.color = Color.white;

    }

    // public void DeleteWhiteSpaces()
    // {
    //     characterNameText.text = "asdf";
    // }

    //! data persistance
    public void InGameSave(ref GameData data)
    {
    }

    public void LoadData(GameData data)
    {
        this.slectedCharacter = data.selectedChar;
        this.charExists = data.charExists;
        this.charNames = data.charNames;
    }
    public void SaveData(ref GameData data)
    {
        data.charExists = this.charExists;
        data.charNames = this.charNames;
    }
}
