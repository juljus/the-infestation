using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Warrior/(2)Shackles")]
public class ShacklesSkill : Skill
{
    public float searchRadius;
    public float stunDuration;
    public float moveDistance;
    public GameObject shacklesPrefab;
    public float rotationsPerSecond;

    private Collider2D[] hitEnemies;
    private float timeLeft;

    // *ABOUT: binds together the target enemy with the closest other enemy, stunning them both.

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {   
        GameObject target = GameObject.Find("GameManager").GetComponent<TargetManager>().GetTargetSmart();
        if (target == null) { return; }
        GameObject.Find("GameManager").GetComponent<TargetManager>().ClearTarget();

        // instantiate the shackles
        GameObject shackles = Instantiate(shacklesPrefab, player.transform.position, quaternion.identity);
        shackles.GetComponent<PlayerShacklesScript>().SetFlyTime(activeTime);
        shackles.GetComponent<PlayerShacklesScript>().SetTarget(target);
        shackles.GetComponent<PlayerShacklesScript>().SetRotationsPerSecond(rotationsPerSecond);

        skillHelper.StartCoroutine(AbilityCoroutine(target));
    }

    private IEnumerator AbilityCoroutine(GameObject target)
    {
        // wait for the projectile to reach the target
        yield return new WaitForSeconds(activeTime);

        // get all enemies in the search radius and select the closest
        hitEnemies = Physics2D.OverlapCircleAll(target.transform.position, searchRadius);
        // if no enemies are found, return
        if (hitEnemies.Length == 0) { yield break; }

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

        Vector2 movePos = target.transform.position + new Vector3(UnityEngine.Random.Range(-moveDistance, moveDistance), UnityEngine.Random.Range(-moveDistance, moveDistance), 0);

        timeLeft = activeTime;
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

        target.GetComponent<EnemyBrain>().Stun();
        closestEnemy.GetComponent<EnemyBrain>().Stun();

        yield return new WaitForSeconds(stunDuration);

        target.GetComponent<EnemyBrain>().UnStun();
        closestEnemy.GetComponent<EnemyBrain>().UnStun();

    }
}
