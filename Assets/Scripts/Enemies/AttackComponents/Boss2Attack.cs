using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Enemy/Attack/Boss2")]
public class Boss2Attack : EnemyAttackBase
{
    [Header("Boss 2 Extra Stats")]
    // general
    [HideInInspector] private GameObject player;
    [HideInInspector] private GameObject gameManager;
    [HideInInspector] private bool justStarted = true;

    // attack
    [HideInInspector] private float attackDurationTimer;

    // ability 1: summon wolves
    public GameObject wolfPrefab;
    public float summonCooldown;
    public float summonDuration;
    public float summonAmount;

    [HideInInspector] private bool summonOnCooldown;
    [HideInInspector] private bool summonInProgress;

    // phase 2
    public float phase2HealthThreshold;
    public float phase2MovementSpeed;
    public float phase2AttackCooldown;
    public float phase2AttackDuration;
    public float phase2AttackDamage;
    public float phase2AttackRange;
    public float phase2StoppingDistance;
    public float phase2AttackDistance;
    public float phase2SummonCooldown;
    public float phase2SummonDuration;
    public float phase2SummonAmount;
    public GameObject phase2WolfPrefab;

    [HideInInspector] private bool phase2;


    public override EnemyAttackBase Clone()
    {
        var clone = ScriptableObject.CreateInstance<Boss2Attack>();

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

        // extra ones
        clone.wolfPrefab = wolfPrefab;
        clone.summonCooldown = summonCooldown;
        clone.summonAmount = summonAmount;
        clone.summonDuration = summonDuration;

        clone.phase2HealthThreshold = phase2HealthThreshold;
        clone.phase2MovementSpeed = phase2MovementSpeed;
        clone.phase2AttackCooldown = phase2AttackCooldown;
        clone.phase2AttackDuration = phase2AttackDuration;
        clone.phase2AttackDamage = phase2AttackDamage;
        clone.phase2AttackRange = phase2AttackRange;
        clone.phase2StoppingDistance = phase2StoppingDistance;
        clone.phase2AttackDistance = phase2AttackDistance;
        clone.phase2SummonCooldown = phase2SummonCooldown;
        clone.phase2SummonDuration = phase2SummonDuration;
        clone.phase2SummonAmount = phase2SummonAmount;
        clone.phase2WolfPrefab = phase2WolfPrefab;

        return clone;
    }

    public override void TryAttack(Transform target, Rigidbody2D rigidBody, float playerDistance, EnemyBrain enemyBrain)
    {
        if (justStarted)
        {
            JustStarted(enemyBrain);
        }

        if (enemyBrain.GetCurrentHealth <= phase2HealthThreshold && phase2 == false)
        {
            phase2 = true;
            enemyBrain.GetEnemyMovement.SetMaxSpeed(phase2MovementSpeed);
            attackCooldown = phase2AttackCooldown;
            attackTime = phase2AttackDuration;
            damage = phase2AttackDamage;
            attackRange = phase2AttackRange;
            summonCooldown = phase2SummonCooldown;
            summonDuration = phase2SummonDuration;
            summonAmount = phase2SummonAmount;
            wolfPrefab = phase2WolfPrefab;

        }

        if (attackInProgress == true || summonInProgress == true)
        {
            return;
        }

        if (summonOnCooldown == false)
        {
            enemyBrain.StartCoroutine(Summon(rigidBody, enemyBrain));
        }
        else if (playerDistance <= attackDistance && attackOnCooldown == false && attackInProgress == false)
        {
            enemyBrain.StartCoroutine(AttackCoroutine(target, rigidBody, enemyBrain));
        }
    }

    private void JustStarted(EnemyBrain enemyBrain)
    {
        gameManager = GameObject.Find("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
        
        enemyBrain.StartCoroutine(SummonCooldown());

        justStarted = false;
    }


    public override IEnumerator AttackCoroutine(Transform target, Rigidbody2D rigidBody, EnemyBrain enemyBrain)
    {
        attackInProgress = true;
        Debug.Log("Attacking");
        
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
        }

        Attack(target, rigidBody, enemyBrain);
    }


    private void Attack(Transform target, Rigidbody2D rigidbody, EnemyBrain enemyBrain)
    {
        // deal damage
        player.GetComponent<PlayerHealth>().TakeDamage(damage);

        // start cooldown
        attackInProgress = false;
        attackOnCooldown = true;
        enemyBrain.StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        attackOnCooldown = true;

        yield return new WaitForSeconds(attackCooldown);

        attackOnCooldown = false;
    }

    private IEnumerator Summon(Rigidbody2D rigidbody, EnemyBrain enemyBrain)
    {
        summonInProgress = true;
        attackInProgress = true;

        for (int i = 0; i < summonAmount; i++)
        {
            yield return new WaitForSeconds(summonDuration);
            Instantiate(wolfPrefab, rigidbody.transform.position, Quaternion.identity);
        }

        summonInProgress = false;
        attackInProgress = false;
        summonOnCooldown = true;
        enemyBrain.StartCoroutine(SummonCooldown());
    }

    private IEnumerator SummonCooldown()
    {
        summonOnCooldown = true;

        yield return new WaitForSeconds(summonCooldown);

        summonOnCooldown = false;
    }
}
