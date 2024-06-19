using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{    
    public int[] slotCharacterTypes = new int[4];
    public int selectedCharacter = 0;
    
    public int[] characterLevels = new int[4];
    public long[] characterExperience = new long[4];

    public bool[][] learnedSkills = new bool[4][] { new bool[15], new bool[15], new bool[15], new bool[15] };
}
