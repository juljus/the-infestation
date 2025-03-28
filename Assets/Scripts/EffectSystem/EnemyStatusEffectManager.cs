using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusEffectManager : MonoBehaviour
{
    private EffectSystem.StatusEffect[] statusEffectList;

    void Update()
    {
        // set enemy stats according to status effects
        SetEnemyStats();
    }

    private void SetEnemyStats()
    {
        // get the status effect array from effect system script
        statusEffectList = transform.GetComponent<EffectSystem>().GetStatusEffectList;

        // get scripts
        EnemyBrain enemyBrain = transform.GetComponent<EnemyBrain>();
        EnemyMovementBase enemyMovementBase = enemyBrain.GetEnemyMovement;
        EnemyAttackBase enemyAttackBase = enemyBrain.GetEnemyAttack;

        // get current stats
        float maxSpeed = enemyMovementBase.GetSpeed;
        float currentHealth = enemyBrain.GetCurrentHealth;
        float attackDamage = enemyAttackBase.GetMaxAttackDamage;
        float attackTime = enemyAttackBase.GetAttackTime;

        // get new values
        float[] recieveValues = new float[5];
        UsedFunctions usedFunctions = new UsedFunctions();
        recieveValues = usedFunctions.SetStatsAccordingToStatusEffects(statusEffectList, maxSpeed, currentHealth, attackDamage, attackTime, 0f);

        // set new values
        enemyMovementBase.SetSpeed(recieveValues[0]);
        enemyBrain.SetCurrentHealth(recieveValues[1]);
        enemyAttackBase.SetAttackDamage(recieveValues[2]);
        enemyAttackBase.SetCurrentAttackTime(recieveValues[3]);
    }
}
