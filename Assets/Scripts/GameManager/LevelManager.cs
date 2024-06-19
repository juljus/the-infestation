using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IDataPersistance
{
    private int[] characterLevels = new int[4];
    private long[] characterExperience = new long[4];
    private int selectedCharacter;
    [SerializeField] private TMPro.TMP_Text levelText;
    [SerializeField] private TMPro.TMP_Text XPText;

    void Start()
    {
        levelText.text = "Level: " + characterLevels[selectedCharacter];
        XPText.text = "XP: " + characterExperience[selectedCharacter];
    }

    public void GainExperiance(long ExperianceGained)
    {
        long experianceForNextLevel = (long)Math.Pow(4, characterLevels[selectedCharacter] + 1);
        long experianceMissing = experianceForNextLevel - characterExperience[selectedCharacter];
        long experianceOverflow = ExperianceGained - experianceMissing;

        if (experianceOverflow >= 0)
        {
            characterLevels[selectedCharacter]++;
            characterExperience[selectedCharacter] = 0;
            GainExperiance(experianceOverflow);
            levelText.text = "Level: " + characterLevels[selectedCharacter];
            XPText.text = "XP: " + characterExperience[selectedCharacter];
        }
        else
        {
            characterExperience[selectedCharacter] += ExperianceGained;
            XPText.text = "XP: " + characterExperience[selectedCharacter];
        }
    }

    public void XPButton()
    {
        GainExperiance(1000);
    }

    public void ResetLevel()
    {
        characterLevels[selectedCharacter] = 0;
        characterExperience[selectedCharacter] = 0;
        levelText.text = "Level: " + characterLevels[selectedCharacter];
        XPText.text = "XP: " + characterExperience[selectedCharacter];
    }

    public int GetPlayerLevel
    {
        get
        {
            return characterLevels[selectedCharacter];
        }
    }

    public void LoadData(GameData data)
    {
        this.characterLevels = data.characterLevels;
        this.characterExperience = data.characterExperience;
        this.selectedCharacter = data.selectedCharacter;
    }

    public void SaveData(ref GameData data)
    {
        data.characterLevels = this.characterLevels;
        data.characterExperience = this.characterExperience;
        data.selectedCharacter = this.selectedCharacter;
    }
}
