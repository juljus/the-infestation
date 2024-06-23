using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Attack/RangedProjectile")]
public class EnemyAttack_RangedProjectile : EnemyAttackBase
{
    [Header("RangedProjectile")]
    public GameObject projectile;
    public float projectileSpeed;


    public override void TryAttack(Transform target, Rigidbody2D rigidBody, float playerDistance, EnemyBrain enemyBrain)
    {
        if (playerDistance <= attackDistance)
        {
            Debug.Log("RangedProjectile TryAttack");
            enemyBrain.StartAttackCoroutine(target, rigidBody, playerDistance, enemyBrain);
        }
    }


    private void Attack(Transform target, Rigidbody2D rigidBody, EnemyBrain enemyBrain)
    {
        GameObject projectileClone = Instantiate(projectile, rigidBody.position, Quaternion.identity);
        projectileClone.transform.parent = rigidBody.transform;

        enemyBrain.StartAttackCooldownCoroutine();
    }


    public override IEnumerator AttackCoroutine(Transform target, Rigidbody2D rigidBody, EnemyBrain enemyBrain)
    {
        Debug.Log("RangedProjectile AttackCoroutine started");
        attackInProgress = true;
        
        attackTimeRemaining = attackTime;
        while (attackTimeRemaining > 0)
        {            
            float playerDistance = Vector2.Distance(target.position, rigidBody.position);
            if (playerDistance > attackRange)
            {
                attackInProgress = false;
                Debug.Log("RangedProjectile AttackCoroutine ended");
                yield break;
            }

            attackTimeRemaining -= Time.deltaTime;
            yield return null;
        }

        Debug.Log("attacked");
        Attack(target, rigidBody, enemyBrain);
        attackInProgress = false;
    }

    public override IEnumerator AttackCooldownCoroutine()
    {
        attackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        attackOnCooldown = false;
    }


    // ---------- GETTERS -----------
    public override float GetProjectileSpeed { get { return projectileSpeed; } }
}
