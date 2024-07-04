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

        attack = attack.Clone();
        movement = movement.Clone();

        // // FOR EDITOR
        // attack.attackInProgress = false;
        // attack.attackOnCooldown = false;
        // attack.attackTimeRemaining = 0;
    }

    void Update()
    {
        // print(this.name + " " + attack.attackInProgress);
        if (attack.attackInProgress == false)
        {
            playerDistance = Vector2.Distance(player.transform.position, transform.position);
            // print(this.name + " is moving");
            movement.Move(player.transform, GetComponent<Rigidbody2D>(), playerDistance);
        }

        if (attack.attackOnCooldown == false && attack.attackInProgress == false)
        {
            playerDistance = Vector2.Distance(player.transform.position, transform.position);
            // print(this.name + " is attacking");
            attack.TryAttack(player.transform, GetComponent<Rigidbody2D>(), playerDistance, this);
        }
    }


    // -------- PUBLIC FUNCTIONS ------------

    public void Death()
    {
        gameManager.GetComponent<MapCompletion>().AddKill();
        gameManager.GetComponent<MapCompletion>().AddStructure();
        
        Destroy(gameObject);
    }

    //--------- GETTERS ---------
    public float GetPlayerDistance { get { return playerDistance; } }

    public EnemyAttackBase GetEnemyAttack { get { return attack; } }
    public EnemyMovementBase GetEnemyMovement { get { return movement; } }
}
