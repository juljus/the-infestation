using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack_Kata", menuName = "Enemy/Attack/Boss3_Kata")]
public class Boss3Attack_Kata : EnemyAttackBase
{
    [Header("Boss 3 Kata Extra Stats")]

    // general
    [HideInInspector] private GameObject player;
    [HideInInspector] private GameObject gameManager;
    [HideInInspector] private bool justStarted = true;

    // ability 1: smash
    public float smashCooldown;
    public float smashCastTime;
    public float smashDistance;
    public float smashDamage;
    public float smashRange;
    public string smashEffectId;
    public float smashEffectDuration;
    public UnityEngine.UI.Image smashEffectIcon;
    public GameObject smashAreaIndicator;

    [HideInInspector] private bool smashOnCooldown;


    public override EnemyAttackBase Clone()
    {
        var clone = ScriptableObject.CreateInstance<Boss3Attack_Kata>();

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
        clone.smashCooldown = smashCooldown;
        clone.smashCastTime = smashCastTime;
        clone.smashDistance = smashDistance;
        clone.smashDamage = smashDamage;
        clone.smashRange = smashRange;
        clone.smashEffectDuration = smashEffectDuration;
        clone.smashEffectIcon = smashEffectIcon;
        clone.smashEffectId = smashEffectId;
        clone.smashAreaIndicator = smashAreaIndicator;

        return clone;
    }

    public override void TryAttack(Transform target, Rigidbody2D rigidBody, float playerDistance, EnemyBrain enemyBrain)
    {
        if (justStarted)
        {
            JustStarted(enemyBrain);
        }


        if (attackInProgress == true)
        {
            return;
        }


        if (smashOnCooldown == false && playerDistance <= smashDistance)
        {
            enemyBrain.StartCoroutine(Smash(enemyBrain, rigidBody));
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
        
        enemyBrain.StartCoroutine(SmashCooldown());

        justStarted = false;
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

        Attack(rigidBody, enemyBrain);
    }


    private void Attack(Rigidbody2D rigidbody, EnemyBrain enemyBrain)
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

    private IEnumerator Smash(EnemyBrain enemyBrain, Rigidbody2D rigidbody)
    {
        attackInProgress = true;

        float smashTimeRemaining = smashCastTime;
        GameObject areaIndicator = Instantiate(smashAreaIndicator, rigidbody.position, Quaternion.identity);

        while (smashTimeRemaining > 0)
        {
            areaIndicator.transform.localScale = new Vector3((1 - smashTimeRemaining / smashCastTime) * 2 * smashRange, (1 - smashTimeRemaining / smashCastTime) * 2 * smashRange, 1);
            
            smashTimeRemaining -= Time.deltaTime;
            yield return null;
        }

        Destroy(areaIndicator);

        // if player in smash radius deal damage and effects
        if (Vector2.Distance(player.transform.position, areaIndicator.transform.position) <= smashRange)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(smashDamage);
            enemyBrain.StartCoroutine(Stun(smashEffectDuration));
            player.GetComponent<EffectSystem>().TakeStatusEffect("aosfijofocmw930cqj290j", "speedMod", 1, smashEffectDuration, smashEffectIcon, false, true, true);
        }

        attackInProgress = false;
        smashOnCooldown = true;
        enemyBrain.StartCoroutine(SmashCooldown());
    }

    private IEnumerator SmashCooldown()
    {
        smashOnCooldown = true;

        yield return new WaitForSeconds(smashCooldown);

        smashOnCooldown = false;
    }

    private IEnumerator Stun(float duration)
    {
        player.GetComponent<PlayerLogic>().Stun();

        yield return new WaitForSeconds(duration);

        player.GetComponent<PlayerLogic>().UnStun();
    }

    // GETTERS
}
