using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player")]
public class PlayerScriptableObject : ScriptableObject
{
    [Header("Player Stats")]
    public float movementSpeed;
    public float health;
    public float attackDamage;
    public float attackTime;
    public GameObject playerSprite;
    public Skill[] skills;
    
}
