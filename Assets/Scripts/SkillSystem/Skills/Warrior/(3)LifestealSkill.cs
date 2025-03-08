using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(menuName = "Skills/Warrior/(3)Lifesteal")]
public class LifestealSkill : Skill
{
    // *ABOUT: Steals HP from target enemy

    public float lifestealAmount;

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {   
        // get target
        GameObject target = GameObject.Find("GameManager").GetComponent<TargetManager>().GetTargetSmart();
        if (target == null) { return; }
        GameObject.Find("GameManager").GetComponent<TargetManager>().ClearTarget();

        // face the target
        player.GetComponent<PlayerMovement>().FaceTarget(target);

        // set player velocity to 0
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        // start the coroutine
        skillHelper.StartCoroutine(AbilityCoroutine(target, player));        
    }

    private IEnumerator AbilityCoroutine(GameObject target, GameObject player)
    {
        // is throwing
        player.GetComponent<PlayerLogic>().SetIfThrowing(true);

        // wait for active time / 2
        yield return new WaitForSeconds(activeTime / 2);

        // steal the life
        target.GetComponent<EnemyBrain>().TakeDamage(lifestealAmount);
        player.GetComponent<PlayerHealth>().Heal(lifestealAmount);

        // wait for the other half of active time
        yield return new WaitForSeconds(activeTime / 2);

        // is not throwing
        player.GetComponent<PlayerLogic>().SetIfThrowing(false);
    }
}
