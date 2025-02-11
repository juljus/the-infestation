using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement", menuName = "Enemy/Movement/PlayerSeeking")]
public class EnemyMovement_PlayerSeeking : EnemyMovementBase
{
    private bool isAggroed = false;

    public override void Move(Transform player, Rigidbody2D rigidBody, float playerDistance)
    {   
        target = player.gameObject;

        if (playerDistance <= aggroRange)
        {
            isAggroed = true;
        }
        else if (playerDistance >= deaggroRange)
        {
            isAggroed = false;
        }

        if (isAggroed && playerDistance > stoppingDistance)
        {
            rigidBody.position = Vector2.MoveTowards(rigidBody.position, target.transform.position, currentSpeed * Time.deltaTime);

            // kill the IsMovingCoroutine
            if (rigidBody.transform.GetComponent<EnemyBrain>().isMovingCoroutineInstance != null)
            {
                rigidBody.transform.GetComponent<EnemyBrain>().StopCoroutine(rigidBody.transform.GetComponent<EnemyBrain>().isMovingCoroutineInstance);
            }

            Debug.Log("PlayerSeeking: Moving");
            // start the IsMovingCoroutine
            rigidBody.transform.GetComponent<EnemyBrain>().isMovingCoroutineInstance = rigidBody.transform.GetComponent<EnemyBrain>().StartCoroutine(rigidBody.transform.GetComponent<EnemyBrain>().IsMovingCoroutine());

        }
    }

    public override EnemyMovementBase Clone()
    {
        var clone = ScriptableObject.CreateInstance<EnemyMovement_PlayerSeeking>();

        // copy over editor stats
        clone.aggroRange = aggroRange;
        clone.deaggroRange = deaggroRange;
        clone.stoppingDistance = stoppingDistance;
        clone.speed = speed;

        clone.currentSpeed = speed;

        return clone;
    }
}
