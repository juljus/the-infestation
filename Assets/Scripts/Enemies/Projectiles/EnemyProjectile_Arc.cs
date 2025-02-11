using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_Arc : MonoBehaviour
{
    private float projectileSpeed;

    private float projectileEffectValue1;
    private string projectileEffectType1;
    private float projectileEffectDuration1;
    private UnityEngine.UI.Image projectileEffectIcon1;
    private bool projectileEffectIsStackable1;
    private bool projectileEffectIsRemovable1;

    private float projectileEffectValue2;
    private string projectileEffectType2;
    private float projectileEffectDuration2;
    private UnityEngine.UI.Image projectileEffectIcon2;
    private bool projectileEffectIsStackable2;
    private bool projectileEffectIsRemovable2;

    private float damage;
    private GameObject target;
    private Vector3 targetPos;
    private Vector3 startPos;
    private float projectileArcHeight;

    void Start()
    {        
        // get from parent object
        EnemyBrain enemyBrain = transform.parent.GetComponent<EnemyBrain>();
        EnemyAttackBase enemyAttack = enemyBrain.GetEnemyAttack;

        projectileSpeed = enemyAttack.GetProjectileSpeed;
        damage = enemyAttack.GetCurrentAttackDamage;
        projectileArcHeight = enemyAttack.GetProjectileArcHeight;
        
        // effect 1
        projectileEffectValue1 = enemyAttack.GetAttackEffectValue1;
        projectileEffectType1 = enemyAttack.GetAttackEffectType1;
        projectileEffectDuration1 = enemyAttack.GetAttackEffectDuration1;
        projectileEffectIcon1 = enemyAttack.GetAttackEffectIcon1;
        projectileEffectIsStackable1 = enemyAttack.GetAttackEffectIsStackable1;
        projectileEffectIsRemovable1 = enemyAttack.GetAttackEffectIsRemovable1;

        // effect 2
        projectileEffectValue2 = enemyAttack.GetAttackEffectValue2;
        projectileEffectType2 = enemyAttack.GetAttackEffectType2;
        projectileEffectDuration2 = enemyAttack.GetAttackEffectDuration2;
        projectileEffectIcon2 = enemyAttack.GetAttackEffectIcon2;
        projectileEffectIsStackable2 = enemyAttack.GetAttackEffectIsStackable2;
        projectileEffectIsRemovable2 = enemyAttack.GetAttackEffectIsRemovable2;

        //get target object
        target = enemyAttack.GetTarget;
        startPos = transform.position;
    }

    void Update()
    {
        targetPos = target.transform.position;

        // // compute the next position, without arc added in
        // Vector3 nextPos = Vector3.MoveTowards(transform.position, targetPos, projectileSpeed * Time.deltaTime);

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
        if (other.gameObject == target) {
            //deal damage to player
            target.GetComponent<PlayerHealth>().TakeDamage(damage);
            //apply status effect 1
            target.GetComponent<EffectSystem>().TakeStatusEffect("ac,do upo,IHPF,HHJIFAUldfhgl", projectileEffectType1, projectileEffectValue1, projectileEffectDuration1, projectileEffectIcon1, projectileEffectIsStackable1, projectileEffectIsRemovable1);
            //apply status effect 2
            target.GetComponent<EffectSystem>().TakeStatusEffect("öadlginoÄPOJÄPEGIHAighaioUHRG", projectileEffectType2, projectileEffectValue2, projectileEffectDuration2, projectileEffectIcon2, projectileEffectIsStackable2, projectileEffectIsRemovable2);
            //destroy projectile
            Destroy(gameObject);
        }
    }
}
