using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //! characters   
    public int selectedChar = 0;
    public bool[] charExists = new bool[4];
    public int[] charKills = new int[4];
    public int[] charLevels = new int[4];
    public string[] charNames = new string[4];
    
    //! skills
    public bool[][] learnedSkills = new bool[4][] { new bool[8], new bool[8], new bool[8], new bool[8] };

    //! map completion
    // HACK: probably dont need those
        // public int[] currentMap = new int[4];
        // public int[] currentStructures = new int[4];

    //! player stats
    public float[] playerAttackDamage = new float[4];
    public float[] playerAttackTime = new float[4];
    public float[] playerAttackRange = new float[4];
    public float[] playerAttackCooldown = new float[4];
    public float[] playerMovementSpeed = new float[4];
    public float[] playerHealth = new float[4];

    //! settings
    public float musicVolume;
    public float sfxVolume;

    //! enemy placement
    public List<float>[] enemyPositionsX = new List<float>[4] { new List<float>(), new List<float>(), new List<float>(), new List<float>() };
    public List<float>[] enemyPositionsY = new List<float>[4] { new List<float>(), new List<float>(), new List<float>(), new List<float>() };
    public List<int>[] enemyIDs = new List<int>[4] { new List<int>(), new List<int>(), new List<int>(), new List<int>() };
}
