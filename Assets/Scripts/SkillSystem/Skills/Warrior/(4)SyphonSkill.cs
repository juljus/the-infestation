using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(menuName = "Skills/Warrior/(4)Syphon")]
public class SyphonSkill : Skill
{
    public float radius;
    public float hpPerUnit;
    public float maxNumTargets;

    private Collider2D[] hitEnemies;
    private float timeLeft;
    private Vector2 playerLastPos;

    // applies a syphon to enemies in target radius and drains hp based on your movement

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {   
        hitEnemies = new Collider2D[(int)maxNumTargets];

        GameObject target = GameObject.Find("GameManager").GetComponent<TargetManager>().GetTarget;
        
        Collider2D[] targetEnemies = Physics2D.OverlapCircleAll(target.transform.position, radius);
        timeLeft = activeTime;

        int counter = 0;
        foreach (Collider2D enemy in targetEnemies)
        {
            if (enemy.tag == "Enemy" && counter < maxNumTargets)
            {
                hitEnemies[counter] = enemy;
                counter++;
            }
        }

        timeLeft = activeTime;
        playerLastPos = player.transform.position;

        skillHelper.StartCoroutine(AbilityCoroutine(player));
    }

    private IEnumerator AbilityCoroutine(GameObject player)
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;

            float distanceDelta = Vector2.Distance(playerLastPos, player.transform.position);
            float dealDamage = distanceDelta * hpPerUnit;
            
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy != null)
                {                    
                    enemy.GetComponent<EnemyBrain>().TakeDamage(dealDamage);
                }
            }

            player.GetComponent<PlayerHealth>().Heal(dealDamage);

            yield return null;
        }
    }
}
