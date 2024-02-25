using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private float projectileSpeed;
    private float projectileEffectValue;
    private string projectileEffectType;
    private float projectileEffectDuration;
    private UnityEngine.UI.Image projectileEffectIcon;
    private float damage;
    private GameObject player;

    void Start()
    {        
        //get from parent object
        projectileSpeed = transform.parent.GetComponent<EnemyAI>().GetProjectileSpeed;
        projectileEffectValue = transform.parent.GetComponent<EnemyAI>().GetProjectileEffectValue;
        projectileEffectType = transform.parent.GetComponent<EnemyAI>().GetProjectileEffectType;
        projectileEffectDuration = transform.parent.GetComponent<EnemyAI>().GetProjectileEffectDuration;
        projectileEffectIcon = transform.parent.GetComponent<EnemyAI>().GetProjectileEffectIcon;
        damage = transform.parent.GetComponent<EnemyAI>().GetDamage;

        //get player object
        player = GameObject.Find("GameManager").GetComponent<PlayerManager>().GetPlayer;

    }

    void Update()
    {
        //move projectile towards player
        transform.position = Vector2.MoveTowards(transform.position, GameObject.Find("GameManager").GetComponent<PlayerManager>().GetPlayerTransform.position, projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == player) {
            //deal damage to player
            player.GetComponent<PlayerLogic>().TakeEffect("damage", damage, 0);
            player.GetComponent<PlayerLogic>().TakeEffect(projectileEffectType, projectileEffectValue, projectileEffectDuration, projectileEffectIcon);
            Destroy(gameObject);
        }
    }
}
