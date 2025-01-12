using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Warrior/Retaliate")]
public class RetaliateSkill : Skill
{
    public float damageToTrigger;

    public UnityEngine.UI.Image effectIcon;
    public BladeSpinSkill weakerBladeSpinSkill;

    private GameObject player;
    private SkillHelper skillHelper;
    private float currentDamageBuffer = 0;

    // Passive: after taking a set amount of damage, release a weaker version of the BladeSpinSkill.

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {
        this.player = player;
        this.skillHelper = skillHelper;

        // add a listener
        player.GetComponent<PlayerHealth>().takeDamageEvent.AddListener(AddDamage);

        // add icon
        player.GetComponent<EffectSystem>().TakeStatusEffect(id, "speedMod", 1, 0, effectIcon, false, false, false);
    }

    public override void Deactivate(GameObject player)
    {
        // remove the listener
        player.GetComponent<PlayerHealth>().takeDamageEvent.RemoveListener(AddDamage);

        // remove icon
        player.GetComponent<EffectSystem>().RemoveStatusEffectById(id);
    }

    private void AddDamage()
    {
        currentDamageBuffer += player.GetComponent<PlayerHealth>().GetLastDamageRecieved;

        while (currentDamageBuffer >= damageToTrigger)
        {
            currentDamageBuffer -= damageToTrigger;

            weakerBladeSpinSkill.Activate(player, skillHelper);
        }
    }
}
