using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusEffectManager : MonoBehaviour
{
    private EffectSystem.StatusEffect[] statusEffectList;

    void Update()
    {
        // set enemy stats according to status effects
        SetEnemySpeed();
    }

    private void SetEnemySpeed()
    {
        // get the status effect array from effect system script
        statusEffectList = transform.GetComponent<EffectSystem>().GetStatusEffectList;

        // get scripts
        EnemyAI enemyAI = transform.GetComponent<EnemyAI>();
        EnemyHealth enemyHealth = transform.GetComponent<EnemyHealth>();

        // get current stats
        float maxSpeed = enemyAI.GetMaxSpeed();
        float currentHealth = enemyHealth.GetCurrentHealth();

        // get new values
        float[] recieveValues = new float[2];
        UsedFunctions usedFunctions = new UsedFunctions();
        recieveValues = usedFunctions.SetStatsAccordingToStatusEffects(statusEffectList, maxSpeed, currentHealth);

        // set new values
        enemyAI.SetSpeed(recieveValues[0]);
        enemyHealth.SetCurrentHealth(recieveValues[1]);



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
