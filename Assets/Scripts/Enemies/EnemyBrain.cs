using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private EnemyMovementBase movement;
    [SerializeField] private EnemyAttackBase attack;
    [SerializeField] private EnemyHealthScriptableObject healthScriptableObject;
    [SerializeField] private UnityEngine.UI.Image healthBar;


    private float maxHealth;
    private float currentHealth;

    private GameObject gameManager;
    private GameObject player;

    private float playerDistance;

    void Start()
    {
        maxHealth = healthScriptableObject.maxHealth;

        currentHealth = maxHealth;

        gameManager = GameObject.Find("GameManager");
        player = gameManager.GetComponent<PlayerManager>().GetPlayer;

        attack = attack.Clone();
        movement = movement.Clone();
    }

    void Update()
    {
        if (attack.attackInProgress == false)
        {
            print("Moving");
            playerDistance = Vector2.Distance(player.transform.position, transform.position);
            movement.Move(player.transform, GetComponent<Rigidbody2D>(), playerDistance);
        }

        if (attack.attackOnCooldown == false && attack.attackInProgress == false)
        {
            print("Attacking");
            playerDistance = Vector2.Distance(player.transform.position, transform.position);
            attack.TryAttack(player.transform, GetComponent<Rigidbody2D>(), playerDistance, this);
        }
    }


    // -------- PUBLIC FUNCTIONS ------------

    public void Death()
    {   
        GameObject.Find("GameManager").GetComponent<EnemyLogicManager>().enemyDeathEvent.Invoke();

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
            GetComponent<EnemyBrain>().Death();
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

    //--------- GETTERS ---------
    public float GetPlayerDistance { get { return playerDistance; } }

    public float GetCurrentHealth { get{ return currentHealth; } }
    public float GetMaxHealth { get { return maxHealth; } }

    public EnemyAttackBase GetEnemyAttack { get { return attack; } }
    public EnemyMovementBase GetEnemyMovement { get { return movement; } }

    //--------- SETTERS ---------
    public void SetCurrentHealth(float newHealth) { currentHealth = newHealth; }
}
