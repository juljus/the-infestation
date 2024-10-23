using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Script : MonoBehaviour
{
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
    private bool attackOnCooldown;
    private bool attackInProgress;
    private float attackDurationTimer;

    // ability 1: summon wolves
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private float summonCooldown;
    [SerializeField] private float summonDuration;
    [SerializeField] private float summonAmount;
    private bool summonOnCooldown;
    private bool summonInProgress;

    // phase 2
    [SerializeField] private float phase2HealthThreshold;
    [SerializeField] private float phase2MovementSpeed;
    [SerializeField] private float phase2AttackCooldown;
    [SerializeField] private float phase2AttackDuration;
    [SerializeField] private float phase2AttackDamage;
    [SerializeField] private float phase2AttackRange;
    [SerializeField] private float phase2StoppingDistance;
    [SerializeField] private float phase2AttackDistance;
    [SerializeField] private float phase2SummonCooldown;
    [SerializeField] private float phase2SummonDuration;
    [SerializeField] private float phase2SummonAmount;
    [SerializeField] private GameObject phase2WolfPrefab;
    private bool phase2;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");

        // HEALTH STUFF ---------------
        currentHealth = maxHealth;
        // ----------------------------
        
        StartCoroutine(SummonCooldown());
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector2.Distance(player.transform.position, transform.position);

        if (currentHealth <= phase2HealthThreshold && phase2 == false)
        {
            phase2 = true;
            movementSpeed = phase2MovementSpeed;
            attackCooldown = phase2AttackCooldown;
            attackDuration = phase2AttackDuration;
            attackDamage = phase2AttackDamage;
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
            StartCoroutine(Summon());
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

        // deal damage
        player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

        // start cooldown
        attackOnCooldown = true;
        attackInProgress = false;
        StartCoroutine(AttackCooldown());
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

    private IEnumerator Summon()
    {
        summonInProgress = true;

        for (int i = 0; i < summonAmount; i++)
        {
            yield return new WaitForSeconds(summonDuration);
            Instantiate(wolfPrefab, transform.position, Quaternion.identity);
        }

        summonOnCooldown = true;
        summonInProgress = false;
        StartCoroutine(SummonCooldown());
    }

    // COROUTINES ---------------------
    private IEnumerator SummonCooldown()
    {
        summonOnCooldown = true;

        yield return new WaitForSeconds(summonCooldown);

        summonOnCooldown = false;
    }

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
}
