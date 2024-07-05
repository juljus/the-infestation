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
        float maxAttackDamage = enemyAttackBase.GetMaxAttackDamage;

        // get new values
        float[] recieveValues = new float[2];
        UsedFunctions usedFunctions = new UsedFunctions();
        recieveValues = usedFunctions.SetStatsAccordingToStatusEffects(statusEffectList, maxSpeed, currentHealth, maxAttackDamage);

        print("speed: " + recieveValues[0]);

        // set new values
        enemyMovementBase.SetSpeed(recieveValues[0]);
        enemyHealth.SetCurrentHealth(recieveValues[1]);
        enemyAttackBase.SetAttackDamage(recieveValues[2]);



        // float newSpeed = transform.GetComponent<EnemyAI>().GetMaxSpeed();
        // for (int i = 0; i < statusEffectList.Length; i++)
        // {
        //     EffectSystem.StatusEffect statusEffect = statusEffectList[i] as EffectSystem.StatusEffect;
        //     if (statusEffect != null && statusEffect.type == "speedMod" && statusEffect.value < 1f)
        //     {
        //         newSpeed *= statusEffectList[i].value;
        //     }
        // }
        // transform.GetComponent<EnemyAI>().SetSpeed(newSpeed);
    }
}
