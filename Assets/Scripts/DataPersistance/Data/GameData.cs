using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{    
    public int playerLevel = 1;

    // char selection
    public int[] slotCharacterTypes = new int[4];

    // char creation
    public int selectedCharacter = 0;
}
