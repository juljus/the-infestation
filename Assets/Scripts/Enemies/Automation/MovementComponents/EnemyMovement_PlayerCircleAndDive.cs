using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Movement/PlayerCircleAndDive")]
public class EnemyMovement_PlayerCircleAndDive : EnemyMovementBase
{
    // public float circleRadiusMin;
    // public float circleRadiusMax;

    private bool isAggroed = false;

    private bool movingAway = false;
    private bool movingTowards = false;

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
            Move(target.transform.position, rigidBody);
        }
    }

    private void Move(Vector2 targetPosition, Rigidbody2D rigidBody)
    {
        float targetDistance = Vector2.Distance(rigidBody.position, targetPosition);

        Vector2 targetDirection = (targetPosition - rigidBody.position).normalized;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;

        if (targetDistance < 6.5 && targetDistance > 4 && movingAway == false && movingTowards == false)
        {
            // change angle to perpendicular to player
            float circleAngle = targetAngle + 90f;

            // move in that direction
            Vector2 moveDirection = Quaternion.Euler(0, 0, circleAngle) * Vector2.up;
            rigidBody.MovePosition(rigidBody.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
        }
        else if (targetDistance > 4)
        {
            movingAway = false;
            movingTowards = true;
            // move towards player
            Vector2 moveDirection = Quaternion.Euler(0, 0, targetAngle) * Vector2.up;
            rigidBody.MovePosition(rigidBody.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
        }
        else 
        {
            movingTowards = false;
            movingAway = true;
            // move away from player
            Vector2 moveDirection = Quaternion.Euler(0, 0, targetAngle + 180f) * Vector2.up;
            rigidBody.MovePosition(rigidBody.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
        }
    }

    public override EnemyMovementBase Clone()
    {
        var clone = ScriptableObject.CreateInstance<EnemyMovement_PlayerCircleAndDive>();

        // copy over editor stats
        clone.aggroRange = aggroRange;
        clone.deaggroRange = deaggroRange;
        clone.stoppingDistance = stoppingDistance;
        clone.speed = speed;

        clone.currentSpeed = speed;

        return clone;
    }
}
