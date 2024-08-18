using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Warrior/(11)AttackPoison")]
public class AttackPoisonSkill : Skill
{
    public UnityEngine.UI.Image effectIcon;
    public float damagePerSecond;
    public float duration;
    public float speedMod;

    // Passive: applies poison on hit, slowing and damaging the enemy.

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {
        // add listener
        player.GetComponent<PlayerAttack>().playerAttackEvent.AddListener(ApplyPoison);

        // add icon
        player.GetComponent<EffectSystem>().TakeStatusEffect(id, "speedMod", 1, 0, effectIcon, false, false, false);
    }

    public override void Deactivate(GameObject player)
    {
        // remove listener
        player.GetComponent<PlayerAttack>().playerAttackEvent.RemoveListener(ApplyPoison);

        // remove icon
        player.GetComponent<EffectSystem>().RemoveStatusEffectById(id);
    }

    private void ApplyPoison()
    {
        Debug.Log("Applying poison");
        GameObject target = GameObject.Find("GameManager").GetComponent<TargetManager>().GetTarget;

        if (target != null)
        {
            Debug.Log("Applying poison to " + target.name);
            target.GetComponent<EffectSystem>().TakeStatusEffect(id, "speedMod", speedMod, duration);
            target.GetComponent<EffectSystem>().TakeStatusEffect(id, "healthMod", damagePerSecond, duration);
        }
    }
}
