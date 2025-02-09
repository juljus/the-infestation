using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Enemy/Attack/RangedProjectile")]
public class EnemyAttack_RangedProjectileArc : EnemyAttackBase
{
    [Header("RangedProjectile")]
    public GameObject projectile;
    public float projectileSpeed;
    public float projectileArcHeight;

    public override void TryAttack(Transform target, Rigidbody2D rigidBody, float playerDistance, EnemyBrain enemyBrain)
    {
        if (playerDistance <= attackDistance)
        {
            enemyBrain.StartCoroutine(AttackCoroutine(target, rigidBody, enemyBrain));
        }

        this.target = target.gameObject;
    }


    public override EnemyAttackBase Clone()
    {
        var clone = ScriptableObject.CreateInstance<EnemyAttack_RangedProjectileArc>();

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


    private void Attack(Transform target, Rigidbody2D rigidBody, EnemyBrain enemyBrain)
    {
        GameObject projectileClone = Instantiate(projectile, new Vector3(rigidBody.position.x, rigidBody.position.y+0.5f, 0), Quaternion.identity);
        projectileClone.transform.parent = rigidBody.transform;

        enemyBrain.StartCoroutine(AttackCooldownCoroutine());
    }


    public override IEnumerator AttackCoroutine(Transform target, Rigidbody2D rigidBody, EnemyBrain enemyBrain)
    {
        attackInProgress = true;
        
        attackTimeRemaining = attackTime;
        while (attackTimeRemaining > 0)
        {            
            float playerDistance = Vector2.Distance(target.position, rigidBody.position);
            if (playerDistance > attackRange)
            {
                attackInProgress = false;
                yield break;
            }

            if (attackTimeRemaining < attackTime/2)
            {
                Attack(target, rigidBody, enemyBrain);
                break;
            }

            attackTimeRemaining -= Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(attackTime/2);

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
    public override float GetProjectileArcHeight { get { return projectileArcHeight; } }
}
