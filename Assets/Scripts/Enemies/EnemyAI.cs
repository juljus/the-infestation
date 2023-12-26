using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    public float aggroRange = 5f;
    public float deaggroRange = 10f;
    public float stoppingDistance = 3f;
    public float speed = 5f;
    public GameObject projectile;
    public float damage;
    public float attackCooldown;
    private float distance;
    private Transform target;
    private bool isAggroed = false;

    void Start()
    {
        PlayerManager playerManager = GameObject.Find("GameManager").GetComponent<PlayerManager>();
        target = playerManager.GetPlayerTransform;
    }

    void Update()
    {
        distance = Vector2.Distance(target.position, transform.position);
        
        if (distance <= aggroRange) {
            isAggroed = true;
        }

        if (distance >= deaggroRange) {
            isAggroed = false;
        }

        if (isAggroed) {
            ChasePlayer();
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

    private void ChasePlayer() {
        if (distance > stoppingDistance) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else {
            AttackPlayer();
        }
    }

    private void AttackPlayer() {
        // Start attack animation
        // Instantiate projectile
    }
}