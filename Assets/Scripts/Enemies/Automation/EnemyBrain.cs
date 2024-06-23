using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private EnemyMovementBase movement;
    [SerializeField] private EnemyAttackBase attack;

    private GameObject gameManager;
    private GameObject player;

    private float playerDistance;

    
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        player = gameManager.GetComponent<PlayerManager>().GetPlayer;

        // FOR EDITOR
        attack.attackInProgress = false;
        attack.attackOnCooldown = false;
        attack.attackTimeRemaining = 0;
    }

    void Update()
    {
        playerDistance = Vector2.Distance(player.transform.position, transform.position);

        if (attack.attackInProgress == false)
        {
            movement.Move(player.transform, GetComponent<Rigidbody2D>(), playerDistance);
        }

        if (attack.attackOnCooldown == false && attack.attackInProgress == false)
        {
            attack.TryAttack(player.transform, GetComponent<Rigidbody2D>(), playerDistance, this);
        }
    }


    // -------- PUBLIC FUNCTIONS ------------

    // private float movementPauseTimeRemaining;
    // public void PauseMovement(float time)
    // {
    //     if (time > movementPauseTimeRemaining)
    //     {
    //         movementPauseTimeRemaining = time;
    //     }
    // }

    public void StartAttackCoroutine(Transform target, Rigidbody2D rigidBody, float playerDistance, EnemyBrain enemyBrain)
    {
        StartCoroutine(attack.AttackCoroutine(target, rigidBody, enemyBrain));
    }

    public void StartAttackCooldownCoroutine()
    {
        StartCoroutine(attack.AttackCooldownCoroutine());
    }


    //--------- GETTERS ---------
    public float GetPlayerDistance { get { return playerDistance; } }

    public EnemyAttackBase GetEnemyAttack { get { return attack; } }
}
