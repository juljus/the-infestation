using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Projectile : MonoBehaviour
{
    private GameObject player;
    private GameObject gameManager;
    private float projectileSpeed;
    private float attackDamage;


    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        player = gameManager.GetComponent<PlayerManager>().GetPlayer;

        projectileSpeed = transform.parent.GetComponent<Boss2Script>().GetProjectileSpeed;
        attackDamage = transform.parent.GetComponent<Boss2Script>().GetAttackDamage;
    }

    void Update()
    {
        // move towards player
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, projectileSpeed * Time.deltaTime);
        print("move towards player at speed: " + projectileSpeed);

        // rotate based on move direction
        Vector3 moveDirection = player.transform.position - transform.position;
        gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == player) {
            //deal damage to player
            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            //destroy projectile
            Destroy(gameObject);
        }
    }
}
