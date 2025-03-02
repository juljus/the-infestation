using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(menuName = "Skills/Warrior/(1)DaggerThrow")]
public class DaggerThrowSkill : Skill
{
    // *ABOUT: Throws a dagger at the target enemy

    public float daggerSpeed;
    public float daggerDamage;
    public float daggerEffectValue;
    public float daggerEffectDuration;
    public bool daggerEffectIsStackable;

    public GameObject daggerPrefab;

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {
        // get the target enemy
        GameObject target = GameObject.Find("GameManager").GetComponent<TargetManager>().GetTargetSmart();

        // clear target from manager
        GameObject.Find("GameManager").GetComponent<TargetManager>().ClearTarget();

        // face the target
        player.GetComponent<PlayerMovement>().FaceTarget(target);

        // start the coroutine
        skillHelper.StartCoroutine(AbilityCoroutine(target, player));
    }

    private IEnumerator AbilityCoroutine(GameObject target, GameObject player)
    {
        // is throwing
        player.GetComponent<PlayerLogic>().SetIfThrowing(true);

        yield return new WaitForSeconds(activeTime / 2);

        // get the player's position
        Vector2 playerPosition = player.transform.position;

        // extra height to account for player sprite
        Vector2 extraHeight = new Vector2(0, 1f);

        // instantiate the dagger
        GameObject dagger = Instantiate(daggerPrefab, playerPosition + extraHeight, Quaternion.identity);

        // set the dagger's attributes
        PlayerDaggerScript daggerScript = dagger.GetComponent<PlayerDaggerScript>();
        
        daggerScript.SetDaggerSpeed(daggerSpeed);
        daggerScript.SetDaggerDamage(daggerDamage);
        daggerScript.SetTarget(target);

        // set the dagger's effects
        daggerScript.SetDaggerEffectValue(daggerEffectValue);
        daggerScript.SetDaggerEffectDuration(daggerEffectDuration);
        daggerScript.SetDaggerEffectIsStackable(daggerEffectIsStackable);

        yield return new WaitForSeconds(activeTime / 2);

        // is not throwing
        player.GetComponent<PlayerLogic>().SetIfThrowing(false);
    }
}
