using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomScript : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image effectIcon;

    // if player touches the enemy, the enemy will attack the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("collision");
        if (collision.gameObject.tag == "Player")
        {
            // apply a DoT effect to the player
            collision.gameObject.GetComponent<EffectSystem>().TakeStatusEffect("kjdgS98OUCQP938HWJFP89sdjfbn", "healthMod", 10, 6, effectIcon, true, true, true);
        }
    }
}
