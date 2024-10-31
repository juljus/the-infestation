using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Projectile : MonoBehaviour
{
    private GameObject player;
    private GameObject gameManager;
    private float projectileSpeed;
    private float attackDamage;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float projectileArcHeight;


    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        player = gameManager.GetComponent<PlayerManager>().GetPlayer;

        projectileSpeed = transform.parent.GetComponent<EnemyBrain>().GetEnemyAttack.GetProjectileSpeed;
        attackDamage = transform.parent.GetComponent<EnemyBrain>().GetEnemyAttack.GetCurrentAttackDamage;
        projectileArcHeight = transform.parent.GetComponent<EnemyBrain>().GetEnemyAttack.GetProjectileArcHeight;

        startPos = transform.position;
    }

    void Update()
    {
        targetPos = player.transform.position;

        // Compute the next position, with arc added in
		float x0 = startPos.x;
		float x1 = targetPos.x;
		float dist = x1 - x0;
		float nextX = Mathf.MoveTowards(transform.position.x, x1, projectileSpeed * Time.deltaTime);
		float baseY = Mathf.Lerp(startPos.y, targetPos.y, (nextX - x0) / dist);
		float arc = projectileArcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
		Vector3 nextPos = new Vector3(nextX, baseY + arc, transform.position.z);
		
		// Rotate to face the next position, and then move there
		transform.rotation = Quaternion.LookRotation(Vector3.forward, nextPos - transform.position);
		transform.position = nextPos;
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
