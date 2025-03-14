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

    // TODO: should trigger when reaches the target, not using wait time

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {   
        GameObject target = GameObject.Find("GameManager").GetComponent<TargetManager>().GetTargetSmart();
        if (target == null) { return; }
        GameObject.Find("GameManager").GetComponent<TargetManager>().ClearTarget();

        // set player velocity to 0
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;


        // face the target
        player.GetComponent<PlayerMovement>().FaceTarget(target);

        skillHelper.StartCoroutine(AnimationCoroutine(target, player, skillHelper));
    }

    private IEnumerator AnimationCoroutine(GameObject target, GameObject player, SkillHelper skillHelper)
    {
        // is throwing
        player.GetComponent<PlayerLogic>().SetIfThrowing(true);

        // wait for half of active time
        yield return new WaitForSeconds(activeTime / 2);

        // extra height to account for player sprite
        Vector3 extraHeight = new Vector3(0, 1f, 0);

        // instantiate the shackles
        GameObject shackles = Instantiate(shacklesPrefab, player.transform.position + extraHeight, quaternion.identity);
        shackles.GetComponent<PlayerShacklesScript>().SetActiveTime(activeTime);
        shackles.GetComponent<PlayerShacklesScript>().SetTarget(target);
        shackles.GetComponent<PlayerShacklesScript>().SetRotationsPerSecond(rotationsPerSecond);
        shackles.GetComponent<PlayerShacklesScript>().SetSearchRadius(searchRadius);
        shackles.GetComponent<PlayerShacklesScript>().SetMoveDistance(moveDistance);
        shackles.GetComponent<PlayerShacklesScript>().SetStunDuration(stunDuration);

        // TEMP: not used
        // skillHelper.StartCoroutine(AbilityCoroutine(target));

        // wait for the other half of active time
        yield return new WaitForSeconds(activeTime / 2);

        // is not throwing
        player.GetComponent<PlayerLogic>().SetIfThrowing(false);
        
        // set animation
        player.GetComponent<PlayerMovement>().StopRunAnimation();
    }

    // TEMP: not used
    // private IEnumerator AbilityCoroutine(GameObject target)
    // {
    //     // wait for the projectile to reach the target
    //     yield return new WaitForSeconds(activeTime);

    //     // get all enemies in the search radius
    //     hitEnemies = Physics2D.OverlapCircleAll(target.transform.position, searchRadius);

    //     // if no enemies are found, return
    //     if (hitEnemies.Length == 0) { yield break; }

    //     // find the closest enemy
    //     float closestDistance = Mathf.Infinity;
    //     GameObject closestEnemy = null;
    //     foreach (Collider2D enemy in hitEnemies)
    //     {
    //         if (enemy.tag == "Enemy" && enemy.gameObject != target)
    //         {
    //             float distance = Vector2.Distance(target.transform.position, enemy.transform.position);
    //             if (distance < closestDistance)
    //             {
    //                 closestDistance = distance;
    //                 closestEnemy = enemy.gameObject;
    //             }
    //         }
    //     }

    //     // apply stuns
    //     if (target == null || closestEnemy == null)
    //     {
    //         yield break;
    //     }

    //     // calculate the position to move the enemy to
    //     Vector2 movePos = target.transform.position + new Vector3(UnityEngine.Random.Range(-moveDistance, moveDistance), UnityEngine.Random.Range(-moveDistance, moveDistance), 0);

    //     // move the enemy to the position
    //     timeLeft = activeTime;
    //     while (timeLeft > 0)
    //     {
    //         timeLeft -= Time.deltaTime;
            
    //         if (closestEnemy != null && target != null)
    //         {
    //             float distance = Vector2.Distance(movePos, closestEnemy.transform.position);
                
    //             closestEnemy.transform.position = Vector2.MoveTowards(closestEnemy.transform.position, movePos, distance * Time.deltaTime / timeLeft);
    //         }

    //         yield return null;
    //     }

    //     target.GetComponent<EnemyBrain>().Stun();
    //     closestEnemy.GetComponent<EnemyBrain>().Stun();

    //     yield return new WaitForSeconds(stunDuration);

    //     target.GetComponent<EnemyBrain>().UnStun();
    //     closestEnemy.GetComponent<EnemyBrain>().UnStun();

    // }
}
