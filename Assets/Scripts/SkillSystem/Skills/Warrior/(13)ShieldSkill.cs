using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Warrior/(13)Shield")]
public class ShieldSkill : Skill
{
    public float shieldAmount;
    public UnityEngine.UI.Image effectIcon;

    private GameObject player;

    private float shieldLeft = 0;

    // Passive: after killing an enemy grants a shield that absorbs damage.

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {
        this.player = player;

        // add listeners
        GameObject.Find("GameManager").GetComponent<EnemyLogicManager>().enemyDeathEvent.AddListener(AddShield);
        player.GetComponent<PlayerHealth>().takeDamageEvent.AddListener(AbsorbDamage);

        // make player not take damage
        player.GetComponent<PlayerHealth>().SetIncomingDamageModForTier4Skills(0);
    }

    public override void Deactivate(GameObject player)
    {
        // remove listeners
        GameObject.Find("GameManager").GetComponent<EnemyLogicManager>().enemyDeathEvent.RemoveListener(AddShield);
        player.GetComponent<PlayerHealth>().takeDamageEvent.RemoveListener(AbsorbDamage);

        // make player take damage
        player.GetComponent<PlayerHealth>().SetIncomingDamageModForTier4Skills(1);

        // remove icon
        player.GetComponent<EffectSystem>().RemoveStatusEffectById(id);
    }

    private void AddShield()
    {
        shieldLeft += shieldAmount;

        if (shieldLeft >= 0)
        {
            // add icon
            player.GetComponent<EffectSystem>().TakeStatusEffect(id, "speedMod", 1, 0, effectIcon, false, false, false);
        }
    }

    private void AbsorbDamage()
    {
        float damageRecieved = player.GetComponent<PlayerHealth>().GetLastDamageRecieved;

        if (shieldLeft >= damageRecieved)
        {
            shieldLeft -= damageRecieved;
            damageRecieved = 0;
        }
        else
        {
            damageRecieved -= shieldLeft;
            shieldLeft = 0;
        }


        if (shieldLeft > 0)
        {
            player.GetComponent<EffectSystem>().TakeStatusEffect(id, "speedMod", 1, 0, effectIcon, false, false, false);
        }
        else
        {
            player.GetComponent<EffectSystem>().RemoveStatusEffectById(id);

            // deal remaining damage
            player.GetComponent<PlayerHealth>().TakeDamage(damageRecieved, true);
        }
    }
}
