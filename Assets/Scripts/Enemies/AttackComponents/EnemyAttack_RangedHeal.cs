using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Enemy/Attack/RangedHeal")]
public class EnemyAttack_RangedHeal : EnemyAttackBase
{
    [Header("RangedProjectile")]
    public GameObject projectile;
    public float projectileSpeed;
    
    public override void TryAttack(Transform player, Rigidbody2D rigidBody, float playerDistance, EnemyBrain enemyBrain)
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

        // calculate target distance
        float targetDistance = Vector2.Distance(target.transform.position, rigidBody.position);

        if (targetDistance <= attackDistance)
        {
            enemyBrain.StartCoroutine(AttackCoroutine(target.transform, rigidBody, enemyBrain));
        }
    }


    public override EnemyAttackBase Clone()
    {
        var clone = ScriptableObject.CreateInstance<EnemyAttack_RangedHeal>();

        // copy over editor stats
        clone.damage = damage;
        clone.attackDistance = attackDistance;
        clone.attackRange = attackRange;
        clone.attackTime = attackTime;
        clone.attackCooldown = attackCooldown;

        clone.attackEffectType1 = attackEffectType1;
        clone.attackEffectValue1 = attackEffectValue1;
        clone.attackEffectDuration1 = attackEffectDuration1;
        clone.attackEffectIcon1 = attackEffectIcon1;
        clone.attackEffectIsStackable1 = attackEffectIsStackable1;
        clone.attackEffectIsRemovable1 = attackEffectIsRemovable1;

        clone.attackEffectType2 = attackEffectType2;
        clone.attackEffectValue2 = attackEffectValue2;
        clone.attackEffectDuration2 = attackEffectDuration2;
        clone.attackEffectIcon2 = attackEffectIcon2;
        clone.attackEffectIsStackable2 = attackEffectIsStackable2;
        clone.attackEffectIsRemovable2 = attackEffectIsRemovable2;

        clone.projectile = projectile;
        clone.projectileSpeed = projectileSpeed;

        return clone;
    }


    public override IEnumerator AttackCoroutine(Transform target, Rigidbody2D rigidBody, EnemyBrain enemyBrain)
    {
            // check if target is null
            if (target == null)
            {
                attackInProgress = false;
                yield break;
            }
        
        attackInProgress = true;
        
        attackTimeRemaining = attackTime;
        while (attackTimeRemaining > 0)
        {
            float targetDistance = Vector2.Distance(target.position, rigidBody.position);
            if (targetDistance > attackRange)
            {
                attackInProgress = false;
                yield break;
            }

            attackTimeRemaining -= Time.deltaTime;
            yield return null;
        }

        Attack(target, rigidBody, enemyBrain);
        attackInProgress = false;
    }


    private void Attack(Transform target, Rigidbody2D rigidBody, EnemyBrain enemyBrain)
    {
        GameObject projectileClone = Instantiate(projectile, rigidBody.position, Quaternion.identity);
        projectileClone.transform.parent = rigidBody.transform;

        enemyBrain.StartCoroutine(AttackCooldownCoroutine());
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
