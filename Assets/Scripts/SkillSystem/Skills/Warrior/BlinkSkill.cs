using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(menuName = "Skills/Warrior/Blink")]
public class BlinkSkill : Skill
{
    public float weaknessDuration;
    public float weaknessMod;

    public float rootDuration;
    public float rootRadius;

    private GameObject target;

    // Teleports to the target enemy and weakens them. Applies root to the enemies around the casting location.

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {   
        target = GameObject.Find("GameManager").GetComponent<TargetManager>().GetTarget;

        // apply root around the player
        Collider2D[] rootHitColliders = Physics2D.OverlapCircleAll(player.transform.position, rootRadius);
        foreach (Collider2D collider in rootHitColliders)
        {
            if (collider.gameObject.tag == "Enemy")
            {
                collider.gameObject.GetComponent<EffectSystem>().TakeStatusEffect(id, "speedMod", 0, rootDuration);
            }
        }

        // teleport to the enemy + 0.5f in a random direction
        player.transform.position = target.transform.position + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f), 0);

        // apply weakness to the target
        target.GetComponent<EffectSystem>().TakeStatusEffect(id, "damageMod", weaknessMod, weaknessDuration);
    }

    // private IEnumerator AbilityCoroutine(GameObject player)
    // {
    //     player.GetComponent<EffectSystem>().TakeStatusEffect(id, "damageMod", damageMod, 0, effectIcon, false, true, false);

    //     UnityEngine.UI.Image buttonOverlay = player.GetComponent<PlayerSkillHolder>().GetSkill2Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();

    //     UnityEngine.UI.Text hitsLeftText = GameObject.Find("GameManager").transform.Find("SkillButtonText").GetComponent<UnityEngine.UI.Text>();

    //     hitsLeftText = Instantiate(hitsLeftText, buttonOverlay.transform.position, buttonOverlay.transform.rotation, buttonOverlay.transform);
    //     hitsLeftText.gameObject.SetActive(true);
        
        
    //     while (timeLeft > 0 && hitsLeft > 0)
    //     {
    //         timeLeft -= Time.deltaTime;

    //         hitsLeftText.text = hitsLeft.ToString();

    //         if (buttonOverlay != null)
    //         {
    //             buttonOverlay.fillAmount = timeLeft / activeTime;
    //         }

    //         yield return null;
    //     }

    //     // destroy hitslefttext
    //     hitsLeftText.gameObject.SetActive(false);

    //     player.GetComponent<EffectSystem>().RemoveStatusEffectById(id);

    //     // destroy the eventlistener
    //     player.GetComponent<PlayerAttack>().playerAttackEvent.RemoveListener(SubstractBackstabHit);

    //     if (timeLeft > 0)
    //     {
    //         player.GetComponent<PlayerSkillHolder>().SkipSkill2ActiveDuration();
    //     }
    // }

    // private void SubstractBackstabHit()
    // {
    //     hitsLeft--;
    // }
}
