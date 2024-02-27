using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusEffectManager : MonoBehaviour
{
    private EffectSystem.StatusEffect[] statusEffectList;

    void Update()
    {
        // get the status effect array from effect system script
        statusEffectList = transform.GetComponent<EffectSystem>().GetStatusEffectList;

        // set enemy stats according to status effects
        SetEnemySpeed();
    }

    private void SetEnemySpeed()
    {
        float newSpeed = transform.GetComponent<EnemyAI>().GetMaxSpeed();
        for (int i = 0; i < statusEffectList.Length; i++)
        {
            EffectSystem.StatusEffect.SpeedModEffect statusEffect = statusEffectList[i] as EffectSystem.StatusEffect.SpeedModEffect;
            if (statusEffect != null && statusEffect.type == "speedMod" && statusEffect.value < 1f)
            {
                newSpeed *= statusEffectList[i].value;
            }
        }
        transform.GetComponent<EnemyAI>().SetSpeed(newSpeed);
    }
}
