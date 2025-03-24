using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShacklesScript : MonoBehaviour
{
    private float activeTime;
    private GameObject target;
    private float rotationsPerSecond;
    private float searchRadius;
    private float moveDistance;
    private float stunDuration;

    private Collider2D[] hitEnemies;
    private float timeLeft;
    private GameObject closestEnemy = null;
    private bool isRotating = true;
    private bool hasStunned = false;


    // Update is called once per frame
    void Update()
    {
        // if the target is null, end the shackles
        if (target == null)
        {
            print("Target is null");
            
            EndShackles(closestEnemy);

            return;
        }

        // if the closest enemy is null after the stun, end the shackles
        if (hasStunned && closestEnemy == null)
        {
            print("Closest enemy is null");

            EndShackles(closestEnemy);

            return;
        }

        // if the shackles have reached the target, move the shackles to the target's position
        if (activeTime <= 0)
        {
            transform.position = target.transform.position;
        }

        // calculate speed
        float shacklesSpeed = Vector2.Distance(transform.position, target.transform.position) / activeTime;
        activeTime -= Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, shacklesSpeed * Time.deltaTime);

        // rotate the shackles at the speed of x rotations per second
        if (isRotating)
        {
            transform.Rotate(0, 0, 360 * rotationsPerSecond * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == target)
        {
            StartCoroutine(ActivationCoroutine());

            isRotating = false;
        }
    }

    private IEnumerator ActivationCoroutine()
    {
        // get all enemies in the search radius
        hitEnemies = Physics2D.OverlapCircleAll(target.transform.position, searchRadius);

        // if no enemies are found, return
        if (hitEnemies.Length == 0) { yield break; }

        // find the closest enemy
        float closestDistance = Mathf.Infinity;
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

        // if no enemies are found, return
        if (closestEnemy == null || target == null)
        { 
            EndShackles(closestEnemy);
            yield break;
        }

        // apply stuns
        target.GetComponent<EnemyBrain>().Stun();
        closestEnemy.GetComponent<EnemyBrain>().Stun();
        hasStunned = true;

        // calculate the position to move the enemy to
        Vector2 movePos = target.transform.position + new Vector3(UnityEngine.Random.Range(-moveDistance, moveDistance), UnityEngine.Random.Range(-moveDistance, moveDistance), 0);

        // move the enemy to the position
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

        // wait for the stun duration
        yield return new WaitForSeconds(stunDuration);

        // end the shackles
        EndShackles(closestEnemy);
    }

    private void EndShackles(GameObject closestEnemy)
    {
        // remove the stuns if they were applied
        if (hasStunned)
        {
            if (target != null) { target.GetComponent<EnemyBrain>().UnStun(); }
            if (closestEnemy != null) { closestEnemy.GetComponent<EnemyBrain>().UnStun(); }
        }

        // destroy the shackles
        Destroy(gameObject);
    }

    // SETTERS
    public void SetActiveTime(float time)
    {
        this.activeTime = time;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void SetRotationsPerSecond(float rotationsPerSecond)
    {
        this.rotationsPerSecond = rotationsPerSecond;
    }

    public void SetSearchRadius(float searchRadius)
    {
        this.searchRadius = searchRadius;
    }

    public void SetMoveDistance(float moveDistance)
    {
        this.moveDistance = moveDistance;
    }

    public void SetStunDuration(float stunDuration)
    {
        this.stunDuration = stunDuration;
    }
}
