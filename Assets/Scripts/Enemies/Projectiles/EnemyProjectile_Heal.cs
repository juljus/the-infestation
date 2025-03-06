using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_Heal : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;

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
    [SerializeField] private GameObject target;

    private float extraTarHeight = 1f;

    void Start()
    {        
        // get from parent object
        EnemyBrain enemyBrain = transform.parent.GetComponent<EnemyBrain>();
        EnemyAttackBase enemyAttack = enemyBrain.GetEnemyAttack;

        projectileSpeed = enemyAttack.GetProjectileSpeed;
        damage = enemyAttack.GetCurrentAttackDamage;
        
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
    }

    void Update()
    {
        // check if target is null
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        //move projectile towards player
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position + new Vector3(0, extraTarHeight, 0), projectileSpeed * Time.deltaTime);

        // rotate based on move direction
        Vector3 moveDirection = target.transform.position - transform.position;
        gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == target) {
            //deal damage to player
            target.GetComponent<EnemyBrain>().Heal(damage);
            //destroy projectile
            Destroy(gameObject);
        }
    }
}
