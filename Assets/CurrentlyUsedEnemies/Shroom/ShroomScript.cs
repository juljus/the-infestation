using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomScript : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image effectIcon;

    [SerializeField] private float attackDamage;
    [SerializeField] private float poisonDamage;
    [SerializeField] private float poisonDuration;

    // if player touches the enemy, the enemy will attack the player
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            // apply damage to player
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

            // apply a DoT effect to the player
            collision.gameObject.GetComponent<EffectSystem>().TakeStatusEffect("kjdgS98OUCQP938HWJFP89sdjfbn", "healthMod", poisonDamage, poisonDuration, effectIcon, true, true, true);
        
            // destroy self
            Destroy(gameObject);
        }
    }
}
