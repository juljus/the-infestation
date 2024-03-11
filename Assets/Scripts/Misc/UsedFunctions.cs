using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UsedFunctions
{
    public float[] SetStatsAccordingToStatusEffects(EffectSystem.StatusEffect[] statusEffectList, float maxSpeed, float currentHealth)
    {
        float[] returnValues = new float[2];
        float newSpeed = maxSpeed;
        float newHealth = currentHealth;

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
                }
            }
        }

        returnValues[0] = newSpeed;
        returnValues[1] = newHealth;
        return returnValues;
    }

}
