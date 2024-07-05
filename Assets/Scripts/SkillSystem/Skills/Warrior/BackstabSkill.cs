using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(menuName = "Skills/Warrior/Backstab")]
public class BackstabSkill : Skill
{
    public float damageMod;
    public float numOfHits;
    public UnityEngine.UI.Image effectIcon;

    private GameObject target;
    private float timeLeft;
    private float hitsLeft;

    // applies a syphon to enemies in target radius and drains hp based on your movement

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {   
        target = GameObject.Find("GameManager").GetComponent<TargetManager>().GetTarget;
        timeLeft = activeTime;
        hitsLeft = numOfHits;

        // teleport to the far side of the enemy
        Vector2 targetPos = target.transform.position;
        Vector2 targetDirection = (targetPos - (Vector2)player.transform.position).normalized;
        Vector2 teleportPos = targetPos + targetDirection;
        player.transform.position = teleportPos;

        skillHelper.StartCoroutine(AbilityCoroutine(player));
    }

    private IEnumerator AbilityCoroutine(GameObject player)
    {
        player.GetComponent<EffectSystem>().TakeStatusEffect(id, "damageMod", damageMod, 0, effectIcon, false, true, false);

        UnityEngine.UI.Image buttonOverlay = player.GetComponent<PlayerSkillHolder>().GetSkill2Button.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>();

        UnityEngine.UI.Text hitsLeftText = GameObject.Find("GameManager").transform.Find("SkillButtonText").GetComponent<UnityEngine.UI.Text>();

        hitsLeftText = Instantiate(hitsLeftText, buttonOverlay.transform.position, buttonOverlay.transform.rotation, buttonOverlay.transform);
        hitsLeftText.gameObject.SetActive(true);
        
        
        while (timeLeft > 0 && hitsLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            hitsLeftText.text = hitsLeft.ToString();

            if (buttonOverlay != null)
            {
                buttonOverlay.fillAmount = timeLeft / activeTime;
            }

            yield return null;
        }

        // destroy hitslefttext
        hitsLeftText.gameObject.SetActive(false);

        player.GetComponent<EffectSystem>().RemoveStatusEffectById(id);
        
        // // draw a red circle around the player
        // Debug.DrawLine(player.transform.position, new Vector3(player.transform.position.x + castRange, player.transform.position.y, player.transform.position.z), Color.red, activeTime);

        // while (timeLeft > 0)
        // {
        //     timeLeft -= Time.deltaTime;

        //     float distanceDelta = Vector2.Distance(playerLastPos, player.transform.position);
        //     float dealDamage = distanceDelta * hpPerUnit;
            
        //     foreach (Collider2D enemy in hitEnemies)
        //     {
        //         if (enemy != null)
        //         {                    
        //             enemy.GetComponent<EnemyHealth>().TakeDamage(dealDamage);
        //         }
        //     }

        //     yield return null;
        // }

        // // delete the circle
        // Debug.DrawLine(player.transform.position, new Vector3(player.transform.position.x + castRange, player.transform.position.y, player.transform.position.z), Color.clear, 0);
    }
}
