using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Speed Archer")]
public class SpeedArcher : Skill
{
    public float speedMod;
    public UnityEngine.UI.Image effectIcon;

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {   
        // get parent effect system
        EffectSystem effectSystem = player.GetComponent<EffectSystem>();

        // apply effect
        effectSystem.TakeStatusEffect("id", "speedMod", speedMod, 0, effectIcon, false, false, false);
    }

    public override void Deactivate(GameObject player)
    {
        // get parent effect system
        EffectSystem effectSystem = player.GetComponent<EffectSystem>();

        // remove dash effect
        effectSystem.RemoveStatusEffectById("id");
    }
}
