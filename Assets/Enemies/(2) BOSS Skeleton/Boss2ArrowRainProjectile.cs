using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss2ArrowRainProjectile : MonoBehaviour
{
    private GameObject gameManager;
    private GameObject player;
    private Vector2 targetPos;
    private float projectileSpeed;
    private float projectileArea;
    private float attackDamage;
    private Vector3 startPos;

    [SerializeField] private float projectileArcHeight;
    [SerializeField] private GameObject projectileTargetAreaMarker;


    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        player = gameManager.GetComponent<PlayerManager>().GetPlayer;

        attackDamage = transform.parent.GetComponent<EnemyBrain>().GetEnemyAttack.GetCurrentAttackDamage;

        // instantiate target area marker
        projectileTargetAreaMarker = Instantiate(projectileTargetAreaMarker, targetPos, Quaternion.identity);

        // set marker scale
        projectileTargetAreaMarker.transform.localScale = new Vector3(projectileArea, projectileArea, 1);

        startPos = transform.position;
    }

    void Update()
    {
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

        // check if projectile has reached target position
        if (transform.position.x >= targetPos.x - 0.05f && transform.position.x <= targetPos.x + 0.05f && transform.position.y >= targetPos.y - 0.05f && transform.position.y <= targetPos.y + 0.05f)
        {
            Terminate();
        }
    }

    private void Terminate()
    {
        // remove target area marker and place explosion marker
        Destroy(projectileTargetAreaMarker);

        // deal damage around itself in a small radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, projectileArea);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == player)
            {
                player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            }
        }

        // destroy projectile
        Destroy(gameObject);
    }

    // SETTERS ------------

    public void SetTargetPos(Vector2 targetPos)
    {
        this.targetPos = targetPos;
    }

    public void SetProjectileArea(float projectileArea)
    {
        this.projectileArea = projectileArea;
    }

    public void SetProjectileSpeed(float projectileSpeed)
    {
        this.projectileSpeed = projectileSpeed;
    }
}
