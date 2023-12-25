using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats")]
public class EnemyStatsScriptableObject : ScriptableObject
{
    public float speed;
    public float maxHealth;
    public float damage;
    public float attackCooldown;
}
