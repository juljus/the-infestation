using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Script : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    [SerializeField] private float stoppingDistance;

    [SerializeField] private float attackDistance;
    [SerializeField] private float attackRange;

    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackDuration;

    [SerializeField] private float attackDamage;

    private bool attackOnCooldown;
    private bool attackInProgress;

    private GameObject player;
    private float playerDistance;

    private float attackDurationTimer;

    private GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector2.Distance(player.transform.position, transform.position);

        if (playerDistance <= attackDistance && attackOnCooldown == false && attackInProgress == false)
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
