using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Warrior/(7)Disperse")]
public class DisperseSkill : Skill
{
    public float damageReflection;
    public float reflectionRadius;

    public UnityEngine.UI.Image effectIcon;

    private GameObject player;

    // Passive: reflects a portion of damage recieved back to nearby enemies.

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {
        this.player = player;

        // set incoming damage mod
        player.GetComponent<PlayerHealth>().SetIncomingDamageModForTier4Skills(1 - damageReflection);

        // add a listener
        player.GetComponent<PlayerHealth>().takeDamageEvent.AddListener(ReflectDamage);

        // add icon
        player.GetComponent<EffectSystem>().TakeStatusEffect(id, "speedMod", 1, 0, effectIcon, false, false, false);
    }

    public override void Deactivate(GameObject player)
    {
        // reset incoming damage mod
        player.GetComponent<PlayerHealth>().SetIncomingDamageModForTier4Skills(1);

        // remove the listener
        player.GetComponent<PlayerHealth>().takeDamageEvent.RemoveListener(ReflectDamage);

        // remove icon
        player.GetComponent<EffectSystem>().RemoveStatusEffectById(id);
    }

    private void ReflectDamage()
    {
        Debug.Log("Reflecting damage");
        float damageRecieved = player.GetComponent<PlayerHealth>().GetLastDamageRecieved;

        int enemiesHitNum = 0;


        // get nearby enemies
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(player.transform.position, reflectionRadius);


        Debug.Log("Enemies in radius: " + hitColliders.Length);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] != null && hitColliders[i].gameObject.CompareTag("Enemy"))
            {
                enemiesHitNum++;
            }
            else
            {
                hitColliders[i] = null;
            }
        }

        // reflect damage
        Debug.Log("Damage recieved: " + damageRecieved);
        Debug.Log("Enemies hit: " + enemiesHitNum);
        float reflectDamage = damageRecieved * damageReflection / enemiesHitNum;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] != null)
            {
                try { hitColliders[i].GetComponent<EnemyBrain>().TakeDamage(reflectDamage); }
                catch {}
                try { hitColliders[i].GetComponent<BossDamageCatcher>().TakeDamage(reflectDamage); }
                catch{}
            }
        }
    }
}
