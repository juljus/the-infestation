using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

// ABOUT: passive - has a chance on each attack to dissapear and launch another attack while invulnerable.
[CreateAssetMenu(menuName = "Skills/Warrior/(14)SleightOfFist")]
public class SleightOfFistSkill : Skill
{
    public float damageMod;
    public float triggerChance;
    public UnityEngine.UI.Image effectIcon;

    private GameObject target;
    private GameObject player;
    private SkillHelper skillHelper;

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {
        this.player = player;
        this.skillHelper = skillHelper;

        // add listener
        player.GetComponent<PlayerAttack>().playerAttackEvent.AddListener(TrySkill);
    }


    private void TrySkill()
    {
        if (UnityEngine.Random.Range(0f, 1f) > triggerChance)
        {
            return;
        }

        target = GameObject.Find("GameManager").GetComponent<TargetManager>().GetTarget;
        if (target == null) { return; }

        // effect icon
        player.GetComponent<EffectSystem>().TakeStatusEffect(id, "speedMod", 1, 0, effectIcon, false, false, false);

        // become invulnerable
        player.GetComponent<PlayerHealth>().SetIfInvulnerable(true);
        player.GetComponent<EffectSystem>().SetIfInvulnerable(true);

        // fade the player sprite
        player.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);

        // start ability coroutine
        skillHelper.StartCoroutine(AbilityCoroutine());
        
    }

    private IEnumerator AbilityCoroutine()
    {
        yield return new WaitForSeconds(activeTime / 2);

        // attack enemy
        if (target != null)
        {
            target.GetComponent<EnemyBrain>().TakeDamage(player.GetComponent<PlayerAttack>().GetCurrentAttackDamage * damageMod);
        }

        yield return new WaitForSeconds(activeTime / 2);

        // become vulnerable
        player.GetComponent<PlayerHealth>().SetIfInvulnerable(false);
        player.GetComponent<EffectSystem>().SetIfInvulnerable(false);

        // unfade the player sprite
        player.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        // remove icon
        player.GetComponent<EffectSystem>().RemoveStatusEffectById(id);
    }
}
