using System.Collections;
using System.Collections.Generic;
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
            rigidBody.position = Vector2.MoveTowards(rigidBody.position, target.position, speed * Time.deltaTime);
        }
    }
}
