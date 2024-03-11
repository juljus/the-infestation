using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{    
    public int[] slotCharacterTypes = new int[4];
    public int selectedCharacter = 0;
    public int[] characterLevels = new int[4];
    public int[] learnedSkills;
}
