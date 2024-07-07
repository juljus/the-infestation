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
        EnemyMovementBase enemyMovementBase = transform.GetComponent<EnemyBrain>().GetEnemyMovement;
        EnemyHealth enemyHealth = transform.GetComponent<EnemyHealth>();
        EnemyAttackBase enemyAttackBase = transform.GetComponent<EnemyBrain>().GetEnemyAttack;

        // get current stats
        float maxSpeed = enemyMovementBase.GetSpeed;
        float currentHealth = enemyHealth.GetCurrentHealth;
        float attackDamage = enemyAttackBase.GetMaxAttackDamage;
        float attackTime = enemyAttackBase.GetAttackTime;

        // get new values
        float[] recieveValues = new float[3];
        UsedFunctions usedFunctions = new UsedFunctions();
        recieveValues = usedFunctions.SetStatsAccordingToStatusEffects(statusEffectList, maxSpeed, currentHealth, attackDamage, attackTime);

        // set new values
        enemyMovementBase.SetSpeed(recieveValues[0]);
        enemyHealth.SetCurrentHealth(recieveValues[1]);
        enemyAttackBase.SetAttackDamage(recieveValues[2]);
        enemyAttackBase.SetCurrentAttackTime(recieveValues[3]);
    }
}
