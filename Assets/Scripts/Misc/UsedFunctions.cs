using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UsedFunctions
{
    public float[] SetStatsAccordingToStatusEffects(EffectSystem.StatusEffect[] statusEffectList, float maxSpeed, float currentHealth, float attackDamage, float attackTime, float attackCooldown)
    {
        float[] returnValues = new float[5];

        float newSpeed = maxSpeed;
        float newHealth = currentHealth;
        float newAttackDamage = attackDamage;
        float newAttackTime = attackTime;
        float newAttackCooldown = attackCooldown;

        for (int i = 0; i < statusEffectList.Length; i++)
        {
            EffectSystem.StatusEffect statusEffect = statusEffectList[i] as EffectSystem.StatusEffect;
            if (statusEffect != null)
            {
                switch (statusEffect.type)
                {
                    case "speedMod":
                        newSpeed *= statusEffect.value;
                        break;
                    case "healthMod":
                        float healthMod = statusEffect.value / statusEffect.duration * Time.deltaTime;
                        newHealth += healthMod;
                        break;
                    case "damageMod":
                        newAttackDamage *= statusEffect.value;
                        break;
                    case "attackTimeMod":
                        newAttackTime *= statusEffect.value;
                        newAttackCooldown *= statusEffect.value;
                        break;
                }
            }
        }

        returnValues[0] = newSpeed;
        returnValues[1] = newHealth;
        returnValues[2] = newAttackDamage;
        returnValues[3] = newAttackTime;
        returnValues[4] = newAttackCooldown;
        return returnValues;
    }

}
