using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private float projectileSpeed;
    private float projectileEffectValue1;
    private string projectileEffectType1;
    private float projectileEffectDuration1;
    private UnityEngine.UI.Image projectileEffectIcon1;
    private float projectileEffectValue2;
    private string projectileEffectType2;
    private float projectileEffectDuration2;
    private UnityEngine.UI.Image projectileEffectIcon2;
    private float damage;
    private GameObject player;

    void Start()
    {        
        // get from parent object
        EnemyBrain enemyBrain = transform.parent.GetComponent<EnemyBrain>();
        EnemyAttackBase enemyAttack = enemyBrain.GetEnemyAttack;

        projectileSpeed = enemyAttack.GetProjectileSpeed;
        damage = enemyAttack.GetDamage;
        // effect 1
        projectileEffectValue1 = enemyAttack.GetAttackEffectValue1;
        projectileEffectType1 = enemyAttack.GetAttackEffectType1;
        projectileEffectDuration1 = enemyAttack.GetAttackEffectDuration1;
        projectileEffectIcon1 = enemyAttack.GetAttackEffectIcon1;
        // effect 2
        projectileEffectValue2 = enemyAttack.GetAttackEffectValue2;
        projectileEffectType2 = enemyAttack.GetAttackEffectType2;
        projectileEffectDuration2 = enemyAttack.GetAttackEffectDuration2;
        projectileEffectIcon2 = enemyAttack.GetAttackEffectIcon2;

        //get player object
        player = GameObject.Find("GameManager").GetComponent<PlayerManager>().GetPlayer;
    }

    void Update()
    {
        //move projectile towards player
        transform.position = Vector2.MoveTowards(transform.position, GameObject.Find("GameManager").GetComponent<PlayerManager>().GetPlayerTransform.position, projectileSpeed * Time.deltaTime);

        // rotate based on move direction
        Vector3 moveDirection = GameObject.Find("GameManager").GetComponent<PlayerManager>().GetPlayerTransform.position - transform.position;
        gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == player) {
            //deal damage to player
            player.GetComponent<PlayerHealth>().TakeDamage(damage);
            //apply status effect 1
            player.GetComponent<EffectSystem>().TakeStatusEffect("ac,do upo,IHPF,HHJIFAUldfhgl", projectileEffectType1, projectileEffectValue1, projectileEffectDuration1, projectileEffectIcon1);
            //apply status effect 2
            player.GetComponent<EffectSystem>().TakeStatusEffect("öadlginoÄPOJÄPEGIHAighaioUHRG", projectileEffectType2, projectileEffectValue2, projectileEffectDuration2, projectileEffectIcon2);
            //destroy projectile
            Destroy(gameObject);
        }
    }
}
