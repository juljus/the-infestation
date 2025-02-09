using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IDataPersistance
{
    private int thisCharLevel;
    [SerializeField] private TMPro.TMP_Text levelText;

    void Start()
    {
        levelText.text = "Level: " + thisCharLevel;
    }

    public void GainLevel()
    {
        thisCharLevel++;
        levelText.text = "Level: " + thisCharLevel;
    }

    public void ResetLevel()
    {
        thisCharLevel = 0;
        levelText.text = "Level: " + thisCharLevel;
    }

    public int GetPlayerLevel
    {
        get
        {
            return thisCharLevel;
        }
    }

    //! Data Persistance

    public void InGameSave(ref GameData data)
    {
        int selectedCharacter = data.selectedChar;

        data.charLevels[selectedCharacter] = thisCharLevel;
    }

    public void LoadData(GameData data)
    {
        int selectedCharacter = data.selectedChar;
        int[] charLevels = data.charLevels;

        this.thisCharLevel = charLevels[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
    }
}
