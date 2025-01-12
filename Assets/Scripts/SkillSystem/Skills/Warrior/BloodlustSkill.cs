using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(menuName = "Skills/Warrior/Bloodlust")]
public class BloodlustSkill : Skill
{
    public float radius;
    public float missingHealthHealPerEnemy;
    public float maxHeal;

    // Heals player based on how many enemies are nearby

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {   
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(player.transform.position, radius);
        
        int buffer = 0;
        for (int i = 0; i < hitEnemies.Length; i++)
        {
            if (hitEnemies[i].tag == "Enemy")
            {
                buffer++;
            }
        }

        float healAmount = buffer * missingHealthHealPerEnemy;

        if (healAmount > maxHeal)
        {
            healAmount = maxHeal;
        }

        float playerMaxHealth = player.GetComponent<PlayerHealth>().GetMaxHealth;
        float playerCurrentHealth = player.GetComponent<PlayerHealth>().GetCurrentHealth;
        float playerMissingHealth = playerMaxHealth - playerCurrentHealth;

        float finalHealAmount = healAmount * playerMissingHealth;

        player.GetComponent<PlayerHealth>().Heal(finalHealAmount);
    }
}
