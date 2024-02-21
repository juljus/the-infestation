using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    [Header("Enemy Stats")]
    public float speed;
    public float maxHealth;
    public float damage;
    public float attackCooldown;
    public float aggroRange;
    public float deaggroRange;
    public float stoppingDistance;
    public GameObject projectile;
    public float projectileSpeed;
    
}
