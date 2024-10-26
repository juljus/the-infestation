using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Script_Kata : MonoBehaviour
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

    // ability 1: smash
    [SerializeField] private float smashCooldown;
    [SerializeField] private float smashCastTime;
    [SerializeField] private float smashDistance;
    [SerializeField] private float smashDamage;
    [SerializeField] private float smashRange;
    [SerializeField] private float smashEffectDuration;
    [SerializeField] private float smashEffectValue;
    [SerializeField] private UnityEngine.UI.Image smashEffectIcon;
    private bool smashInProgress;
    private bool smashOnCooldown;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        player = gameManager.GetComponent<PlayerManager>().GetPlayer;

        StartCoroutine(SmashCooldown());

        // HEALTH STUFF ---------------
        currentHealth = maxHealth;
        // ----------------------------
    }

    void Update()
    {
        playerDistance = Vector2.Distance(transform.position, player.transform.position);

        if (attackInProgress == true || smashInProgress == true)
        {
            return;
        }

        if (playerDistance <= smashDistance && smashOnCooldown == false)
        {
            Smash();
        }
        else if (playerDistance <= attackDistance && attackOnCooldown == false)
        {
            Attack();
        }
        else if (playerDistance > stoppingDistance)
        {
            Move();
        }
        else
        {
            // do nothing
        }
    }

    private void Move()
    {
        if (playerDistance > stoppingDistance)
        {
            // move towards player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
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

        // start cooldown
        attackInProgress = false;
        attackOnCooldown = true;
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        attackOnCooldown = true;

        yield return new WaitForSeconds(attackCooldown);

        attackOnCooldown = false;
    }

    private IEnumerator Smash()
    {
        smashInProgress = true;

        yield return new WaitForSeconds(smashCastTime);

        // if player in smash radius deal damage and effects
        if (playerDistance <= smashRange)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            player.GetComponent<EffectSystem>().TakeStatusEffect("daghvibcnlw03eöh38diJW98", "attackTimeMod", smashEffectValue, smashEffectDuration, smashEffectIcon, false, true, true);
        }

        // start cooldown
        smashInProgress = false;
        smashOnCooldown = true;
        StartCoroutine(SmashCooldown());
    }

    private IEnumerator SmashCooldown()
    {
        smashOnCooldown = true;

        yield return new WaitForSeconds(smashCooldown);

        smashOnCooldown = false;
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
