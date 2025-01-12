using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(menuName = "Skills/Warrior/(3)Lifesteal")]
public class LifestealSkill : Skill
{
    // ABOUT: Steals HP from target enemy

    public float lifestealAmount;

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {   
        // get target
        GameObject target = GameObject.Find("GameManager").GetComponent<TargetManager>().GetTargetSmart();

        // steal the life
        target.GetComponent<EnemyBrain>().TakeDamage(lifestealAmount);
        player.GetComponent<PlayerHealth>().Heal(lifestealAmount);
        
    }
}
