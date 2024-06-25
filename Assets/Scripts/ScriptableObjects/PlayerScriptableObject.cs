using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player")]
public class PlayerScriptableObject : ScriptableObject
{
    [Header("Player Stats")]
    public float speed;
    public float maxHealth;
    public float attackDamage;
    public GameObject playerSprite;
    public Skill[] skills;
    
}
