using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Movement/Player Seeking")]
public class EnemyMovement_PlayerSeeking : EnemyMovementBase
{
    private bool isAggroed = false;

    public override void Move(Transform target, Rigidbody2D rigidBody, float playerDistance)
    {   
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
            rigidBody.position = Vector2.MoveTowards(rigidBody.position, target.position, currentSpeed * Time.deltaTime);
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
