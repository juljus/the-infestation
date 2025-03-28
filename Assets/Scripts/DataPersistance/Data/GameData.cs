using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //! misc
    public bool tutorialComplete = false;
    public bool[] storyShown = new bool[4];

    //! characters   
    public int selectedChar = 0;
    public bool[] charExists = new bool[4];
    public bool[] charDead = new bool[4];
    public int[] charKills = new int[4];
    public string[] charNames = new string[4];
    public float[][] charCoords = new float[4][] { new float[2], new float[2], new float[2], new float[2] };
    
    //! skills
    public int[] charLevels = new int[4];
    public bool[][] learnedSkills = new bool[4][] { new bool[8], new bool[8], new bool[8], new bool[8] };

    //! player stats
    public float[] playerAttackDamage = new float[4];
    public float[] playerAttackTime = new float[4];
    public float[] playerAttackRange = new float[4];
    public float[] playerAttackCooldown = new float[4];
    public float[] playerMovementSpeed = new float[4];
    public float[] playerHealth = new float[4];
    public float[] playerHealthRegen = new float[4];

    //! settings
    public float musicVolume;
    public float sfxVolume;

    //! enemy placement
    public List<float>[] enemyPositionsX = new List<float>[4] { new List<float>(), new List<float>(), new List<float>(), new List<float>() };
    public List<float>[] enemyPositionsY = new List<float>[4] { new List<float>(), new List<float>(), new List<float>(), new List<float>() };
    public List<int>[] enemyIDs = new List<int>[4] { new List<int>(), new List<int>(), new List<int>(), new List<int>() };

    // // map discovery tiles
    // public List<float>[] discoverableTilesX = new List<float>[4] { new List<float>(), new List<float>(), new List<float>(), new List<float>() };
    // public List<float>[] discoverableTilesY = new List<float>[4] { new List<float>(), new List<float>(), new List<float>(), new List<float>() };
}
