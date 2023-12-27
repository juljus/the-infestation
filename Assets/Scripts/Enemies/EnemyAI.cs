using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject scriptableObject;
    [SerializeField] private Rigidbody2D rigidBody;
    private float aggroRange;
    private float deaggroRange;
    private float stoppingDistance;
    private float speed;
    private GameObject projectile;
    private float damage;
    private float attackCooldown;
    private float distance;
    private Transform target;
    private bool isAggroed = false;
    private float attackCooldownRemaining;

    void Start()
    {
        PlayerManager playerManager = GameObject.Find("GameManager").GetComponent<PlayerManager>();
        target = playerManager.GetPlayerTransform;

        aggroRange = scriptableObject.aggroRange;
        deaggroRange = scriptableObject.deaggroRange;
        stoppingDistance = scriptableObject.stoppingDistance;
        speed = scriptableObject.speed;
        projectile = scriptableObject.projectile;
        damage = scriptableObject.damage;
        attackCooldown = scriptableObject.attackCooldown;

        attackCooldownRemaining = attackCooldown;
    }

    void Update()
    {
        attackCooldownRemaining -= Time.deltaTime;
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
        else if (attackCooldownRemaining <= 0) {
            AttackPlayer();
        }
    }

    private void AttackPlayer() {
        // Start attack animation

        GameObject projectileClone = Instantiate(projectile, transform.position, Quaternion.identity);
        projectileClone.transform.parent = transform;
        attackCooldownRemaining = attackCooldown;
    }
}