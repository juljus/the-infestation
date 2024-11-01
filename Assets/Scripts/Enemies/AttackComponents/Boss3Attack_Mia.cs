using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Enemy/Attack/Boss3_Mia")]
public class Boss3Attack_Mia : EnemyAttackBase
{
    // The second half of the 3rd boss. More agile and tactical, but still melee focused.

    [Header("Boss 3 Mia Extra Stats")]

    // general
    [HideInInspector] private GameObject player;
    [HideInInspector] private GameObject gameManager;
    [HideInInspector] private bool justStarted = true;

    // ability 1: strike (run through the player while dealing damage and applying an effect)
    public float strikeCooldown;
    public float strikeCastTime;
    public float strikeDistance;
    public float strikeDamage;
    public float strikeRange;
    public float strikeSpeed;
    public float strikeRunThroughDistance;
    public float strikeDamageRadius;
    public string strikeEffectId;
    public float strikeEffectDuration;
    public UnityEngine.UI.Image strikeEffectIcon;

    [HideInInspector] private bool strikeOnCooldown;



    public override EnemyAttackBase Clone()
    {
        var clone = ScriptableObject.CreateInstance<Boss3Attack_Mia>();

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
        clone.strikeCooldown = strikeCooldown;
        clone.strikeCastTime = strikeCastTime;
        clone.strikeDistance = strikeDistance;
        clone.strikeDamage = strikeDamage;
        clone.strikeRange = strikeRange;
        clone.strikeSpeed = strikeSpeed;
        clone.strikeRunThroughDistance = strikeRunThroughDistance;
        clone.strikeDamageRadius = strikeDamageRadius;
        clone.strikeEffectId = strikeEffectId;
        clone.strikeEffectDuration = strikeEffectDuration;
        clone.strikeEffectIcon = strikeEffectIcon;

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

        if (strikeOnCooldown == false && playerDistance <= strikeDistance)
        {
            enemyBrain.StartCoroutine(Strike(enemyBrain, rigidBody));
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
        
        enemyBrain.StartCoroutine(StrikeCooldown());

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

    private IEnumerator Strike(EnemyBrain enemyBrain, Rigidbody2D rigidbody)
    {
        attackInProgress = true;

        yield return new WaitForSeconds(strikeCastTime);

        // establish the endpoint
        Vector2 playerPosition = player.transform.position;
        Vector2 enemyPosition = rigidbody.position;
        Vector2 direction = playerPosition - enemyPosition;
        Vector2 playerDistance = direction;
        direction.Normalize();

        Vector2 endPoint = enemyPosition + direction * strikeRunThroughDistance + playerDistance;

        // move towards the endpoint at strikeSpeed
        while (Vector2.Distance(rigidbody.position, endPoint) > 0.1f)
        {
            rigidbody.MovePosition(Vector2.MoveTowards(rigidbody.position, endPoint, strikeSpeed * Time.deltaTime));
            yield return null;
        }

        attackInProgress = false;
        strikeOnCooldown = true;
        enemyBrain.StartCoroutine(StrikeCooldown());
    }

    private IEnumerator StrikeCooldown()
    {
        strikeOnCooldown = true;

        yield return new WaitForSeconds(strikeCooldown);

        strikeOnCooldown = false;
    }

    // GETTERS
}
