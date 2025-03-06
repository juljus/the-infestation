using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement", menuName = "Enemy/Movement/EnemySeekingHealer")]
public class EnemyMovement_EnemySeekingHealer : EnemyMovementBase
{
    private bool isAggroed = false;

    public override void Move(Transform player, Rigidbody2D rigidBody, float playerDistance)
    {
        // select closest enemy with missing health but not self
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        target = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(enemy.transform.position, rigidBody.position);
            if (distance < closestDistance && enemy.GetComponent<EnemyBrain>().GetCurrentHealth < enemy.GetComponent<EnemyBrain>().GetMaxHealth && enemy != rigidBody.gameObject)
            {
                closestDistance = distance;
                target = enemy;
            }
        }

        // if no enemy with missing health select the closest but not self
        if (target == null)
        {
            closestDistance = Mathf.Infinity;
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(enemy.transform.position, rigidBody.position);
                if (distance < closestDistance && enemy != rigidBody.gameObject)
                {
                    closestDistance = distance;
                    target = enemy;
                }
            }
        }

        if (target == null)
        {
            return;
        }

        // set facing direction
        if (target.transform.position.x < rigidBody.position.x)
        {
            rigidBody.transform.GetChild(0).localScale = new Vector3(1, 1, 1);
            rigidBody.transform.GetChild(0).GetChild(0).localScale = new Vector3(1, 1, 1);
        }
        else
        {
            rigidBody.transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
            rigidBody.transform.GetChild(0).GetChild(0).localScale = new Vector3(-1, 1, 1);
        }

        // calculate target distance
        float targetDistance = Vector2.Distance(target.transform.position, rigidBody.position);

        if (targetDistance <= aggroRange)
        {
            isAggroed = true;
        }
        else if (targetDistance >= deaggroRange)
        {
            isAggroed = false;
        }

        if (isAggroed && targetDistance > stoppingDistance)
        {
            rigidBody.position = Vector2.MoveTowards(rigidBody.position, target.transform.position, currentSpeed * Time.deltaTime);

            // kill the IsMovingCoroutine
            if (rigidBody.transform.GetComponent<EnemyBrain>().isMovingCoroutineInstance != null)
            {
                rigidBody.transform.GetComponent<EnemyBrain>().StopCoroutine(rigidBody.transform.GetComponent<EnemyBrain>().isMovingCoroutineInstance);
            }

            // start the IsMovingCoroutine
            rigidBody.transform.GetComponent<EnemyBrain>().isMovingCoroutineInstance = rigidBody.transform.GetComponent<EnemyBrain>().StartCoroutine(rigidBody.transform.GetComponent<EnemyBrain>().IsMovingCoroutine());
        }
    }

    public override EnemyMovementBase Clone()
    {
        var clone = ScriptableObject.CreateInstance<EnemyMovement_EnemySeekingHealer>();

        // copy over editor stats
        clone.aggroRange = aggroRange;
        clone.deaggroRange = deaggroRange;
        clone.stoppingDistance = stoppingDistance;
        clone.speed = speed;

        clone.currentSpeed = speed;

        return clone;
    }
}
