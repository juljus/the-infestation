using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Archer/Suppress")]
public class SuppressSkill : Skill
{
    public float speedMod;
    public UnityEngine.UI.Image effectIcon;

    // removes all slow effects and applies speedup

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {   
        // get parent effect system
        EffectSystem effectSystem = player.GetComponent<EffectSystem>();

        // remove all slow effects
        effectSystem.RemoveStatusEffectByTypeAndValue("speedMod", false);

        // apply speedup
        effectSystem.TakeStatusEffect(id, "speedMod", speedMod, activeTime, effectIcon);
    }
}
