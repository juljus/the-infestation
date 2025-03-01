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
    [SerializeField] private Animator animator;

    // TODO: somewhere here the movement animation should also be triggered

    private float maxHealth;
    private float currentHealth;

    private GameObject gameManager;
    private GameObject player;

    private float playerDistance;

    private int isStunned;

    // public IEnumerator isMovingCoroutine;

    public Coroutine isMovingCoroutineInstance;

    void Start()
    {
        maxHealth = healthScriptableObject.maxHealth;

        currentHealth = maxHealth;

        gameManager = GameObject.Find("GameManager");
        player = gameManager.GetComponent<PlayerManager>().GetPlayer;

        attack = attack.Clone();
        movement = movement.Clone();

        // isMovingCoroutine = IsMovingCoroutine();
    }

    void Update()
    {
        if (isStunned > 0)
        {
            return;
        }

        if (attack.attackInProgress == false)
        {
            playerDistance = Vector2.Distance(player.transform.position, transform.position);
            movement.Move(player.transform, GetComponent<Rigidbody2D>(), playerDistance);
        }

        if (attack.attackOnCooldown == false && attack.attackInProgress == false)
        {
            playerDistance = Vector2.Distance(player.transform.position, transform.position);
            attack.TryAttack(player.transform, GetComponent<Rigidbody2D>(), playerDistance, this);
        }

        if (attack.attackInProgress == true)
        {
            // attack animation
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }

        // // check if velocity is more than 0
        // if (GetComponent<Rigidbody2D>().velocity.magnitude > 0)
        // {
        //     animator.SetBool("isMoving", true);
        // }
        // else
        // {
        //     animator.SetBool("isMoving", false);
        // }

        // check if target is to the left or right ( if not healer )
        if (attack.GetType() != typeof(EnemyAttack_RangedHeal))
        {
            if (player.transform.position.x < transform.position.x)
            {
                transform.GetChild(0).localScale = new Vector3(1, 1, 1);
                transform.GetChild(0).GetChild(0).localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
                transform.GetChild(0).GetChild(0).localScale = new Vector3(-1, 1, 1);
            }
        }
    }


    // -------- PUBLIC FUNCTIONS ------------

    // HACK: this is totally not good practice, but i didnt manage to think of a better way to do this
    public IEnumerator IsMovingCoroutine()
    {
        print("isRunning true");
        animator.SetBool("isRunning", true);
        yield return new WaitForSeconds(0.1f);
        print("isRunning false");
        animator.SetBool("isRunning", false);
    }

    public void Death()
    {   
        GameObject.Find("GameManager").GetComponent<EnemyLogicManager>().enemyDeathEvent.Invoke();

        gameManager.GetComponent<MapCompletion>().AddKill();

        // if the enemy was the boss, then trigger the end sequence
        if (gameObject.tag == "Boss")
        {
            gameManager.GetComponent<MapCompletion>().EndSequence();
            return;
        }
        
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

    //--------- GETTERS ---------
    public float GetPlayerDistance { get { return playerDistance; } }

    public float GetCurrentHealth { get{ return currentHealth; } }
    public float GetMaxHealth { get { return maxHealth; } }

    public EnemyAttackBase GetEnemyAttack { get { return attack; } }
    public EnemyMovementBase GetEnemyMovement { get { return movement; } }

    //--------- SETTERS ---------
    public void SetCurrentHealth(float newHealth)
    {
        currentHealth = newHealth;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (currentHealth <= 0)
        {
            Death();
        }

        healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void SetIfChanneling(bool isChanneling)
    {
        if (isChanneling)
        {
            animator.SetBool("isChanneling", true);
        }
        else
        {
            animator.SetBool("isChanneling", false);
        }
    }

    public void Stun() { isStunned ++; }
    public void UnStun() { isStunned --; }
}
