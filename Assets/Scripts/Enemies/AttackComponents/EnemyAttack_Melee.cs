using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Enemy/Attack/Melee")]
public class EnemyAttack_Melee : EnemyAttackBase
{
    public override void TryAttack(Transform target, Rigidbody2D rigidBody, float playerDistance, EnemyBrain enemyBrain)
    {
        if (playerDistance <= attackDistance)
        {
            enemyBrain.StartCoroutine(AttackCoroutine(target, rigidBody, enemyBrain));
        }
    }


    public override EnemyAttackBase Clone()
    {
        var clone = ScriptableObject.CreateInstance<EnemyAttack_Melee>();

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

        return clone;
    }


    private void Attack(Transform target, Rigidbody2D rigidBody, EnemyBrain enemyBrain)
    {
        GameObject player = GameObject.Find("GameManager").GetComponent<PlayerManager>().GetPlayer;

        //deal damage to player
        player.GetComponent<PlayerHealth>().TakeDamage(damage);
        //apply status effect 1
        player.GetComponent<EffectSystem>().TakeStatusEffect("wöofjcaöow09ujp0dson<jon ", attackEffectType1, attackEffectValue1, attackEffectDuration1, attackEffectIcon1, attackEffectIsStackable1, attackEffectIsRemovable1);
        //apply status effect 2
        player.GetComponent<EffectSystem>().TakeStatusEffect("öCJ Ü90I+ D9 uoh  osoiv0ew8husv", attackEffectType2, attackEffectValue2, attackEffectDuration2, attackEffectIcon2, attackEffectIsStackable2, attackEffectIsRemovable2);

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

            attackTimeRemaining -= Time.deltaTime;
            yield return null;
        }

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
}
