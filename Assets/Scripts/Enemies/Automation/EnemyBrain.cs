using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    }

    void Update()
    {
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
    }


    // -------- PUBLIC FUNCTIONS ------------

    public void Death()
    {   
        GameObject.Find("GameManager").GetComponent<EnemyLogicManager>().enemyDeathEvent.Invoke();

        gameManager.GetComponent<MapCompletion>().AddKill();
        gameManager.GetComponent<MapCompletion>().AddStructure();
        
        Destroy(gameObject);
    }

    //--------- GETTERS ---------
    public float GetPlayerDistance { get { return playerDistance; } }

    public EnemyAttackBase GetEnemyAttack { get { return attack; } }
    public EnemyMovementBase GetEnemyMovement { get { return movement; } }
}
