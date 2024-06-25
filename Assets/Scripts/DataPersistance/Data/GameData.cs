using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // characters   
    public int[] slotCharacterTypes = new int[4];
    public int selectedCharacter = 0;
    
    // levels
    public int[] characterLevels = new int[4];
    public long[] characterExperience = new long[4];

    // skills
    public bool[][] learnedSkills = new bool[4][] { new bool[15], new bool[15], new bool[15], new bool[15] };

    // map completion
    public int currentMap = 0;
    public int currentKills = 0;
    public int currentStructures = 0;

    // boss
    public int lastBossId = 0;
}
