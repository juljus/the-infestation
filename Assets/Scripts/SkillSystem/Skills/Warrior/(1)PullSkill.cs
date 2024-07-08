using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Warrior/(1)Pull")]
public class PullSkill : Skill
{
    public float range;
    public float WeaknessMod;
    public float WeaknessDuration;

    private Collider2D[] hitEnemies;
    private float timeLeft;

    // pulls all enemies in range towards you, then weakens them

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {   
        hitEnemies = Physics2D.OverlapCircleAll(player.transform.position, range);
        timeLeft = activeTime;

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                enemy.GetComponent<EffectSystem>().TakeStatusEffect(id, "speedMod", 0, activeTime);
            }
        }

        skillHelper.StartCoroutine(AbilityCoroutine(player));
    }

    private IEnumerator AbilityCoroutine(GameObject player)
    {
        // draw a red circle around the player
        Debug.DrawLine(player.transform.position, new Vector3(player.transform.position.x + range, player.transform.position.y, player.transform.position.z), Color.red, activeTime);

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy != null && enemy.tag == "Enemy")
                {
                    float distance = Vector2.Distance(player.transform.position, enemy.transform.position);
                    
                    enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, player.transform.position, distance * Time.deltaTime / timeLeft);
                }
            }

            yield return null;
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy != null && enemy.tag == "Enemy")
            {
                enemy.GetComponent<EffectSystem>().TakeStatusEffect(id, "damageMod", WeaknessMod, WeaknessDuration);
            }
        }

        // delete the circle
        Debug.DrawLine(player.transform.position, new Vector3(player.transform.position.x + range, player.transform.position.y, player.transform.position.z), Color.clear, 0);
    }
}
