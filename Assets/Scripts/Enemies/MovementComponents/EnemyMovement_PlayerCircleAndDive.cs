using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// BUG: this will not set the moving variable on the enemy animator

[CreateAssetMenu(fileName = "Movement", menuName = "Enemy/Movement/PlayerCircleAndDive")]
public class EnemyMovement_PlayerCircleAndDive : EnemyMovementBase
{

    private bool isAggroed = false;

    private bool isDiving = false;
    private bool isCooldown = false;
    private float diveCooldown;
    private float diveCooldownRemaining;

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

        if (isCooldown)
        {
            diveCooldownRemaining -= Time.deltaTime;
        }

        if (isCooldown && diveCooldownRemaining <= 0)
        {
            isCooldown = false;


        }

        MoveExt(target.transform.position, rigidBody);
    }

    private void MoveExt(Vector2 targetPosition, Rigidbody2D rigidBody)
    {
        float targetDistance = Vector2.Distance(rigidBody.position, targetPosition);

        Vector2 targetDirection = (targetPosition - rigidBody.position).normalized;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;

        // not diving
        if ((isCooldown || (targetDistance > stoppingDistance + 0.5f || targetDistance < stoppingDistance - 0.5f)) && isDiving == false)
        {
            // circle around player
            if (targetDistance < stoppingDistance + 0.5f && targetDistance > stoppingDistance - 0.5f)
            {
                // change angle perpendicular to player
                float circleAngle = targetAngle + 90f;

                // move in that direction
                Vector2 moveDirection = Quaternion.Euler(0, 0, circleAngle) * Vector2.up;
                rigidBody.MovePosition(rigidBody.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
            }
            // move away from player
            else if (targetDistance < stoppingDistance)
            {
                // move away from player diagonally
                float moveAwayAngle = targetAngle - 60f;

                Vector2 moveDirection = Quaternion.Euler(0, 0, moveAwayAngle + 180f) * Vector2.up;
                rigidBody.MovePosition(rigidBody.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
            }
            // move towards player
            else
            {            
                // move towards player diagonally
                float movingTowardsAngle = targetAngle + 60f;

                Vector2 moveDirection = Quaternion.Euler(0, 0, movingTowardsAngle) * Vector2.up;
                rigidBody.MovePosition(rigidBody.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
            }
        }
        // start diving
        else if (isCooldown == false && isDiving == false && targetDistance <= stoppingDistance + 0.5f && targetDistance >= stoppingDistance - 0.5f)
        {
            isDiving = true;
        }
        // diving
        else if (isDiving)
        {
            // move towards player
            Vector2 moveDirection = Quaternion.Euler(0, 0, targetAngle) * Vector2.up;
            rigidBody.MovePosition(rigidBody.position + moveDirection * currentSpeed * Time.fixedDeltaTime);
        }
        // stop diving
        if (isDiving && targetDistance < 0.2)
        {
            isDiving = false;
            diveCooldown = rigidBody.gameObject.GetComponent<EnemyBrain>().GetEnemyAttack.GetAttackCooldown;
            diveCooldownRemaining = diveCooldown;
            isCooldown = true;
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
