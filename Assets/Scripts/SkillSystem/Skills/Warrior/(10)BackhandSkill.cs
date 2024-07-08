using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Warrior/(10)Backhand")]
public class BackhandSkill : Skill
{
    public UnityEngine.UI.Image effectIcon;

    // Passive: converts attackTime to attack cooldown.

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {
        player.GetComponent<PlayerAttack>().SetAnimationToCooldown(true);

        // add icon
        player.GetComponent<EffectSystem>().TakeStatusEffect(id, "speedMod", 1, 0, effectIcon, false, false, false);
    }

    public override void Deactivate(GameObject player)
    {
        player.GetComponent<PlayerAttack>().SetAnimationToCooldown(false);

        // remove icon
        player.GetComponent<EffectSystem>().RemoveStatusEffectById(id);
    }
}
