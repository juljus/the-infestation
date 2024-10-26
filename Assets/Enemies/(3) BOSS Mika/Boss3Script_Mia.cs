using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Script_Mia : MonoBehaviour
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

    void Start()
    {
        gameManager = GameObject.Find("GameManager");

        // HEALTH STUFF ---------------
        currentHealth = maxHealth;
        // ----------------------------
    }

    void Update()
    {
        playerDistance = Vector2.Distance(transform.position, player.transform.position);
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
