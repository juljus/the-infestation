using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // characters   
    public int[] slotCharacterTypes = new int[4];
    public bool[] isFirstBoot = new bool[4];
    public int selectedCharacter = 0;
    
    // levels
    public int[] characterLevels = new int[4];
    public long[] characterExperience = new long[4];

    // skills
    public bool[][] learnedSkills = new bool[4][] { new bool[8], new bool[8], new bool[8], new bool[8] };

    // map completion
    public int[] currentMap = new int[4];
    public int[] currentKills = new int[4];
    public int[] currentStructures = new int[4];

    // boss
    public int lastBossId = 0;

    // player stats
    public float[] playerAttackDamage = new float[4];
    public float[] playerAttackTime = new float[4];
    public float[] playerAttackRange = new float[4];
    public float[] playerAttackCooldown = new float[4];
    public float[] playerMovementSpeed = new float[4];
    public float[] playerHealth = new float[4];

    // settings
    public float musicVolume;
    public float sfxVolume;

    // enemy placement
    public List<float> enemyPositionsX = new List<float>();
    public List<float> enemyPositionsY = new List<float>();
    public List<int> enemyIDs = new List<int>();
}
