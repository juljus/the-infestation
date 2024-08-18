using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Warrior/(2)Shackles")]
public class ShacklesSkill : Skill
{
    public float searchRadius;
    public float stunDuration;
    public float moveDistance;

    private Collider2D[] hitEnemies;
    private float timeLeft;

    // binds together the target enemy with the closest other enemy, stunning them both.

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {   
        GameObject target = GameObject.Find("GameManager").GetComponent<TargetManager>().GetTarget;
        if (target == null)
        {
            GameObject.Find("GameManager").GetComponent<TargetManager>().TargetClosestEnemy();
            target = GameObject.Find("GameManager").GetComponent<TargetManager>().GetTarget;
        }

        hitEnemies = Physics2D.OverlapCircleAll(target.transform.position, searchRadius);
        timeLeft = activeTime;

        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy" && enemy.gameObject != target)
            {
                float distance = Vector2.Distance(target.transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.gameObject;
                }
            }
        }

        skillHelper.StartCoroutine(AbilityCoroutine(target, closestEnemy));
    }

    private IEnumerator AbilityCoroutine(GameObject target, GameObject closestEnemy)
    {
        Vector2 movePos = target.transform.position + new Vector3(UnityEngine.Random.Range(-moveDistance, moveDistance), UnityEngine.Random.Range(-moveDistance, moveDistance), 0);

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            
            if (closestEnemy != null && target != null)
            {
                float distance = Vector2.Distance(movePos, closestEnemy.transform.position);
                
                closestEnemy.transform.position = Vector2.MoveTowards(closestEnemy.transform.position, movePos, distance * Time.deltaTime / timeLeft);
            }

            yield return null;
        }

        // apply stuns
        if (target == null || closestEnemy == null)
        {
            yield break;
        }
        target.GetComponent<EffectSystem>().TakeStatusEffect(id, "stun", 0, stunDuration);
        closestEnemy.GetComponent<EffectSystem>().TakeStatusEffect(id, "stun", 0, stunDuration);        
    }
}
