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

    [Header("Enemy Projectile")]
    public GameObject projectile;
    public float projectileSpeed;

    [Header("Enemy Projectile Effect 1")]
    public string projectileEffectType1;
    public float projectileEffectValue1;
    public float projectileEffectDuration1;
    public UnityEngine.UI.Image projectileEffectIcon1;
    
    [Header("Enemy Projectile Effect 2")]
    public string projectileEffectType2;
    public float projectileEffectValue2;
    public float projectileEffectDuration2;
    public UnityEngine.UI.Image projectileEffectIcon2;
}
