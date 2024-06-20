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
    private float maxSpeed;
    private float speed;
    private GameObject projectile;
    private float projectileSpeed;
    private float damage;
    private float attackCooldown;
    private float distance;
    private Transform target;
    private bool isAggroed = false;
    private float attackCooldownRemaining;
    private float projectileEffectValue1;
    private string projectileEffectType1;
    private float projectileEffectDuration1;
    private UnityEngine.UI.Image projectileEffectIcon1;
    private float projectileEffectValue2;
    private string projectileEffectType2;
    private float projectileEffectDuration2;
    private UnityEngine.UI.Image projectileEffectIcon2;

    void Start()
    {
        PlayerManager playerManager = GameObject.Find("GameManager").GetComponent<PlayerManager>();
        target = playerManager.GetPlayerTransform;

        aggroRange = scriptableObject.aggroRange;
        deaggroRange = scriptableObject.deaggroRange;
        stoppingDistance = scriptableObject.stoppingDistance;
        maxSpeed = scriptableObject.speed;
        projectile = scriptableObject.projectile;
        damage = scriptableObject.damage;
        attackCooldown = scriptableObject.attackCooldown;

        speed = maxSpeed;

        // projectile
        projectileSpeed = scriptableObject.projectileSpeed;
        // effect 1
        projectileEffectValue1 = scriptableObject.projectileEffectValue1;
        projectileEffectType1 = scriptableObject.projectileEffectType1;
        projectileEffectDuration1 = scriptableObject.projectileEffectDuration1;
        projectileEffectIcon1 = scriptableObject.projectileEffectIcon1;
        // effect 2
        projectileEffectValue2 = scriptableObject.projectileEffectValue2;
        projectileEffectType2 = scriptableObject.projectileEffectType2;
        projectileEffectDuration2 = scriptableObject.projectileEffectDuration2;
        projectileEffectIcon2 = scriptableObject.projectileEffectIcon2;

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

    // Getters 
    public float GetProjectileSpeed {
        get { return projectileSpeed; }
    }
    public float GetDamage {
        get { return damage; }
    }

    // effect1
    public float GetProjectileEffectValue1 {
        get { return projectileEffectValue1; }
    }
    public string GetProjectileEffectType1 {
        get { return projectileEffectType1; }
    }
    public float GetProjectileEffectDuration1 {
        get { return projectileEffectDuration1; }
    }
    public UnityEngine.UI.Image GetProjectileEffectIcon1 {
        get { return projectileEffectIcon1; }
    }
    // effect2
    public float GetProjectileEffectValue2 {
        get { return projectileEffectValue2; }
    }
    public string GetProjectileEffectType2 {
        get { return projectileEffectType2; }
    }
    public float GetProjectileEffectDuration2 {
        get { return projectileEffectDuration2; }
    }
    public UnityEngine.UI.Image GetProjectileEffectIcon2 {
        get { return projectileEffectIcon2; }
    }
    public float GetMaxSpeed() {
        return maxSpeed;
    }

    // Setters
    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }
}