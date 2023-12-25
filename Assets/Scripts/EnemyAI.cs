using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float lookRadius = 5f;
    public float stoppingDistance = 1f;
    public float speed = 5f;
    private float distance;
    Transform target;
    private PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("GameManager").GetComponent<PlayerManager>();
        target = playerManager.GetPlayerTransform;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(target.position, transform.position);
        
        if (distance <= lookRadius && distance > stoppingDistance) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }

        if (distance <= lookRadius) {
            // Attack
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}