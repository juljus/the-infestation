using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Dash")]
public class DashSkill : Skill
{
    public float dashSpeedMod;
    public float dashDuration;
    public UnityEngine.UI.Image effectIcon;

    public override void Activate(GameObject player)
    {   
        // get parent effect system
        EffectSystem effectSystem = player.GetComponent<EffectSystem>();

        // apply dash effect
        effectSystem.TakeStatusEffect(id, "speedMod", dashSpeedMod, dashDuration, effectIcon);
    }
}
