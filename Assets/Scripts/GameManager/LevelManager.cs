using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IDataPersistance
{
    private int[] characterLevels = new int[4];
    private int selectedCharacter;
    [SerializeField] private TMPro.TMP_Text levelText;

    void Start()
    {
        levelText.text = "Level: " + characterLevels[selectedCharacter];
    }

    public void GainLevel()
    {
        characterLevels[selectedCharacter]++;
        levelText.text = "Level: " + characterLevels[selectedCharacter];
    }

    public void ResetLevel()
    {
        characterLevels[selectedCharacter] = 0;
        levelText.text = "Level: " + characterLevels[selectedCharacter];
    }

    public int GetPlayerLevel
    {
        get
        {
            return characterLevels[selectedCharacter];
        }
    }

    //! Data Persistance
    // TODO: implement InGameSave in the form of campfires and then move the needed saves to ingamesave instead of save

    public void InGameSave(ref GameData data)
    {
    }

    public void LoadData(GameData data)
    {
        this.characterLevels = data.charLevels;
        this.selectedCharacter = data.selectedChar;
    }

    public void SaveData(ref GameData data)
    {
        data.charLevels = this.characterLevels;
        data.selectedChar = this.selectedCharacter;
    }
}
