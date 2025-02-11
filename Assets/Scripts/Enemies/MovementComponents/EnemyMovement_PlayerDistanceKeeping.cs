using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// BUG:this will not set the moving variable on the enemy animator

[CreateAssetMenu(fileName = "Movement", menuName = "Enemy/Movement/PlayerDistanceKeeping")]
public class EnemyMovement_PlayerDistanceKeeping : EnemyMovementBase
{
    // NB: This one can NOT be edited in the editor
    private float runDistance = 3f;
    // --------------------------------------------


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
        }

        if (playerDistance <= runDistance)
        {
            rigidBody.position = Vector2.MoveTowards(rigidBody.position, target.transform.position, -currentSpeed * Time.deltaTime);
        }
    }

    public override EnemyMovementBase Clone()
    {
        var clone = ScriptableObject.CreateInstance<EnemyMovement_PlayerDistanceKeeping>();

        // copy over editor stats
        clone.aggroRange = aggroRange;
        clone.deaggroRange = deaggroRange;
        clone.stoppingDistance = stoppingDistance;
        clone.speed = speed;

        clone.currentSpeed = speed;

        return clone;
    }
}
