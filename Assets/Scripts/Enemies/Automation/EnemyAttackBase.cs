using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBase : ScriptableObject
{
    // -------- EDITOR STATS --------
    public float damage;
    public float attackDistance;
    public float attackRange;
    public float attackTime;
    public float attackCooldown;

    [Header("Attack Effect 1")]
    public string attackEffectType1;
    public float attackEffectValue1;
    public float attackEffectDuration1;
    public UnityEngine.UI.Image attackEffectIcon1;
    
    [Header("Attack Effect 2")]
    public string attackEffectType2;
    public float attackEffectValue2;
    public float attackEffectDuration2;
    public UnityEngine.UI.Image attackEffectIcon2;


    // -------- VARIABLES --------
    [HideInInspector] public float currentDamage;
    [HideInInspector] public float currentAttackTime;

    [HideInInspector] public bool attackOnCooldown = false;
    [HideInInspector] public float attackTimeRemaining;
    [HideInInspector] public bool attackInProgress = false;


    // -------- GETTERS ----------
    public float GetMaxAttackDamage { get { return damage; } }
    public float GetCurrentAttackDamage { get { return currentDamage; } }
    public float GetAttackTime { get { return attackTime; } }

    public string GetAttackEffectType1 { get { return attackEffectType1; } }
    public float GetAttackEffectValue1 { get { return attackEffectValue1; } }
    public float GetAttackEffectDuration1 { get { return attackEffectDuration1; } }
    public UnityEngine.UI.Image GetAttackEffectIcon1 { get { return attackEffectIcon1; } }

    public string GetAttackEffectType2 { get { return attackEffectType2; } }
    public float GetAttackEffectValue2 { get { return attackEffectValue2; } }
    public float GetAttackEffectDuration2 { get { return attackEffectDuration2; } }
    public UnityEngine.UI.Image GetAttackEffectIcon2 { get { return attackEffectIcon2; } }


    // --------- SETTERS ---------
    public void SetAttackDamage(float newDamage) { currentDamage = newDamage; }
    public void SetCurrentAttackTime(float newCurrentAttackTime) { currentAttackTime = newCurrentAttackTime; }


    // --------- VIRTUAL GETTERS ------------
    public virtual float GetProjectileSpeed { get { return 0; } }


    // -------- VIRTUAL FUNCTIONS --------
    public virtual void TryAttack(Transform target, Rigidbody2D rigidBody, float playerDistance, EnemyBrain enemyBrain) {}

    public virtual EnemyAttackBase Clone() { return null; }

    public virtual IEnumerator AttackCoroutine(Transform target, Rigidbody2D rigidBody, EnemyBrain enemyBrain) { yield return null; }
    public virtual IEnumerator AttackCooldownCoroutine() { yield return null; }
}
