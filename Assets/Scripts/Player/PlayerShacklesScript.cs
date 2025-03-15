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
    private bool isRotating = true;

    // Update is called once per frame
    void Update()
    {
        if (activeTime <= 0)
        {
            // move the shackles to the target
            if (target != null)
            {
                transform.position = target.transform.position;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // calculate speed
        float shacklesSpeed = Vector2.Distance(transform.position, target.transform.position) / activeTime;
        activeTime -= Time.deltaTime;

        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, shacklesSpeed * Time.deltaTime);

            // rotate the shackles at the speed of x rotations per second
            if (isRotating)
            {
                transform.Rotate(0, 0, 360 * rotationsPerSecond * Time.deltaTime);
            }
        }
        else
        {
            Destroy(gameObject);
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

        // return, if no enemies are found
        if (target == null || closestEnemy == null)
        {
            Destroy(gameObject);
            yield break;
        }

        // apply stuns
        target.GetComponent<EnemyBrain>().Stun();
        closestEnemy.GetComponent<EnemyBrain>().Stun();

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

        // remove the stuns
        target.GetComponent<EnemyBrain>().UnStun();
        closestEnemy.GetComponent<EnemyBrain>().UnStun();

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
