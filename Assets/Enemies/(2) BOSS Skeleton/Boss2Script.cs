using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Script : MonoBehaviour
{
    // this boss is a skeleton with a bow. Tt will shoot arrows at the player and up in the air, making it rain arrows. It can also root the player.

    // general
    private GameObject player;
    private GameObject gameManager;

    // movement
    [SerializeField] private float movementSpeed;
    [SerializeField] private float stoppingDistance;
    private float playerDistance;

    // attack
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackDamage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject arrowPrefab;
    private bool attackOnCooldown;
    private bool attackInProgress;
    private float attackDurationTimer;

    // ability 1: arrow rain
    [SerializeField] private GameObject arrowRainArrowPrefab;
    [SerializeField] private float arrowRainCooldown;
    [SerializeField] private float arrowRainDuration;
    [SerializeField] private int arrowRainAmount;
    [SerializeField] private float arrowRainArea;
    [SerializeField] private float arrowRainArrowSpeed;
    [SerializeField] private float arrowRainArrowDamage;
    [SerializeField] private float arrowRainArrowArea;
    [SerializeField] private float arrowRainTriggerDistance;
    private bool arrowRainOnCooldown;
    private bool arrowRainInProgress;

    // ability 2: root
    [SerializeField] private float rootCooldown;
    [SerializeField] private float rootDuration;
    [SerializeField] private float rootRange;
    [SerializeField] private float rootCastTime;
    [SerializeField] private UnityEngine.UI.Image rootIcon;
    private bool rootOnCooldown;
    private bool rootInProgress;

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        player = gameManager.GetComponent<PlayerManager>().GetPlayer;

        // HEALTH STUFF ---------------
        currentHealth = maxHealth;
        // ----------------------------
    }

    void Update()
    {
        playerDistance = Vector2.Distance(transform.position, player.transform.position);

        if (attackInProgress == true || rootInProgress == true || arrowRainInProgress == true)
        {
            return;
        }

        if (rootOnCooldown == false)
        {
            StartCoroutine(Root());
        }
        else if (arrowRainOnCooldown == false && playerDistance <= arrowRainTriggerDistance)
        {
            StartCoroutine(ArrowRain());
        }
        else if (playerDistance <= attackDistance && attackOnCooldown == false && attackInProgress == false)
        {
            Attack();
        }
        else
        {
            Move();
        }
    }

    private void Attack()
    {
        attackInProgress = true;
        attackDurationTimer = attackDuration;

        // attack animation
        while (attackDurationTimer > 0)
        {
            if (playerDistance > attackRange)
            {
                attackInProgress = false;
                return;
            }

            attackDurationTimer -= Time.deltaTime;
        }

        // spawn projectile
        GameObject projectile = Instantiate(arrowPrefab, transform.position, transform.rotation, transform);

        // start cooldown
        attackOnCooldown = true;
        attackInProgress = false;
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator Root()
    {
        rootInProgress = true;

        yield return new WaitForSeconds(rootCastTime);
        
        // root player
        player.GetComponent<EffectSystem>().TakeStatusEffect("lksajf938hcbo8yaG2378D", "speedMod", 0, rootDuration, rootIcon, false, true, true);

        rootInProgress = false;
        rootOnCooldown = true;
        StartCoroutine(RootCooldown());
    }

    private IEnumerator RootCooldown()
    {
        rootOnCooldown = true;

        yield return new WaitForSeconds(rootCooldown);

        rootOnCooldown = false;
    }

    private IEnumerator ArrowRain()
    {
        arrowRainInProgress = true;

        for (int i = 0; i < arrowRainAmount; i++)
        {
            Vector2 randomPosition = new Vector2(Random.Range(transform.position.x - arrowRainArea, transform.position.x +arrowRainArea), Random.Range(transform.position.y - arrowRainArea, transform.position.y + arrowRainArea));
            GameObject arrow = Instantiate(arrowRainArrowPrefab, transform.position, transform.rotation, transform);
            arrow.GetComponent<Boss2ArrowRainProjectile>().SetTargetPos(randomPosition);
            arrow.GetComponent<Boss2ArrowRainProjectile>().SetProjectileArea(arrowRainArrowArea);
            arrow.GetComponent<Boss2ArrowRainProjectile>().SetProjectileSpeed(arrowRainArrowSpeed);

            yield return new WaitForSeconds(arrowRainDuration/arrowRainAmount);
        }

        arrowRainInProgress = false;
        arrowRainOnCooldown = true;
        StartCoroutine(ArrowRainCooldown());
    }

    private IEnumerator ArrowRainCooldown()
    {
        arrowRainOnCooldown = true;

        yield return new WaitForSeconds(arrowRainCooldown);

        arrowRainOnCooldown = false;
    }

    private void Move()
    {
        if (playerDistance > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
        }
        else 
        {
            // do nothing
        }
    }


    // COROUTINES --------------------
    private IEnumerator AttackCooldown()
    {
        attackOnCooldown = true;

        yield return new WaitForSeconds(attackCooldown);

        attackOnCooldown = false;
    }

    // HEALT STUFF --------------------
    [SerializeField] private float maxHealth;
    [SerializeField] private UnityEngine.UI.Image healthBar;
    private float currentHealth;

    public void Death()
    {   
        gameManager.GetComponent<EnemyLogicManager>().enemyDeathEvent.Invoke();
        gameManager.GetComponent<MapCompletion>().AddKill();
        gameManager.GetComponent<MapCompletion>().AddStructure();
        
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Heal(float heal)
    {
        currentHealth += heal;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        healthBar.fillAmount = currentHealth / maxHealth;        
    }
    // --------------------------------

    // GETTERS ------------------------
    public float GetArrowRainArrowSpeed
    {
        get { return arrowRainArrowSpeed; }
    }

    public float GetArrowRainArrowDamage
    {
        get { return arrowRainArrowDamage; }
    }

    public float GetArrowRainArrowArea
    {
        get { return arrowRainArrowArea; }
    }

    public float GetProjectileSpeed
    {
        get { return projectileSpeed; }
    }

    public float GetAttackDamage
    {
        get { return attackDamage; }
    }
}
