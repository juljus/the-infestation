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
    public float projectileSpeed;
    public GameObject arrowPrefab;
    public float projectileArcHeight;

    // ability 1: arrow rain
    public GameObject arrowRainArrowPrefab;
    public float arrowRainCooldown;
    public float arrowRainDuration;
    public int arrowRainAmount;
    public float arrowRainArea;
    public float arrowRainArrowSpeed;
    public float arrowRainArrowDamage;
    public float arrowRainArrowArea;
    public float arrowRainTriggerDistance;
    [HideInInspector] private bool arrowRainOnCooldown;

    // ability 2: root
    public float rootCooldown;
    public float rootDuration;
    public float rootCastTime;
    public UnityEngine.UI.Image rootIcon;
    [HideInInspector] private bool rootOnCooldown;


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
        clone.projectileSpeed = projectileSpeed;
        clone.arrowPrefab = arrowPrefab;
        clone.projectileArcHeight = projectileArcHeight;

        clone.arrowRainArrowPrefab = arrowRainArrowPrefab;
        clone.arrowRainCooldown = arrowRainCooldown;
        clone.arrowRainDuration = arrowRainDuration;
        clone.arrowRainAmount = arrowRainAmount;
        clone.arrowRainArea = arrowRainArea;
        clone.arrowRainArrowSpeed = arrowRainArrowSpeed;
        clone.arrowRainArrowDamage = arrowRainArrowDamage;
        clone.arrowRainArrowArea = arrowRainArrowArea;
        clone.arrowRainTriggerDistance = arrowRainTriggerDistance;

        clone.rootCooldown = rootCooldown;
        clone.rootDuration = rootDuration;
        clone.rootCastTime = rootCastTime;
        clone.rootIcon = rootIcon;

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

        if (rootOnCooldown == false)
        {
            enemyBrain.StartCoroutine(Root(enemyBrain));
        }
        else if (arrowRainOnCooldown == false && playerDistance <= arrowRainTriggerDistance)
        {
            enemyBrain.StartCoroutine(ArrowRain(enemyBrain, rigidBody));
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
        
        enemyBrain.StartCoroutine(RootCooldown());
        enemyBrain.StartCoroutine(ArrowRainCooldown());

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
        // spawn projectile
        Instantiate(arrowPrefab, rigidbody.transform.position, rigidbody.transform.rotation, rigidbody.transform);

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

    private IEnumerator Root(EnemyBrain enemyBrain)
    {
        attackInProgress = true;

        yield return new WaitForSeconds(rootCastTime);
        
        // root player
        player.GetComponent<EffectSystem>().TakeStatusEffect("lksajf938hcbo8yaG2378D", "speedMod", 0, rootDuration, rootIcon, false, true, true);

        attackInProgress = false;
        rootOnCooldown = true;
        enemyBrain.StartCoroutine(RootCooldown());
    }

    private IEnumerator RootCooldown()
    {
        rootOnCooldown = true;

        yield return new WaitForSeconds(rootCooldown);

        rootOnCooldown = false;
    }

    private IEnumerator ArrowRain(EnemyBrain enemyBrain, Rigidbody2D rigidbody)
    {
        attackInProgress = true;

        for (int i = 0; i < arrowRainAmount; i++)
        {
            Vector2 randomPosition = new Vector2(Random.Range(rigidbody.transform.position.x - arrowRainArea, rigidbody.transform.position.x +arrowRainArea), Random.Range(rigidbody.transform.position.y - arrowRainArea, rigidbody.transform.position.y + arrowRainArea));
            GameObject arrow = Instantiate(arrowRainArrowPrefab, rigidbody.transform.position, rigidbody.transform.rotation, rigidbody.transform);
            arrow.GetComponent<Boss2ArrowRainProjectile>().SetTargetPos(randomPosition);
            arrow.GetComponent<Boss2ArrowRainProjectile>().SetProjectileArea(arrowRainArrowArea);
            arrow.GetComponent<Boss2ArrowRainProjectile>().SetProjectileSpeed(arrowRainArrowSpeed);

            yield return new WaitForSeconds(arrowRainDuration/arrowRainAmount);
        }

        attackInProgress = false;
        arrowRainOnCooldown = true;
        enemyBrain.StartCoroutine(ArrowRainCooldown());
    }

    private IEnumerator ArrowRainCooldown()
    {
        arrowRainOnCooldown = true;

        yield return new WaitForSeconds(arrowRainCooldown);

        arrowRainOnCooldown = false;
    }

    // GETTERS
    public override float GetProjectileSpeed { get { return projectileSpeed; } }
    public override float GetProjectileArcHeight { get { return projectileArcHeight; } }
}
