using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// *ABOUT: passive - applies poison on hit, slowing and damaging the enemy.
[CreateAssetMenu(menuName = "Skills/Warrior/(11)AttackPoison")]
public class AttackPoisonSkill : Skill
{
    public UnityEngine.UI.Image effectIcon;
    public float poisonDamage;
    public float speedMod;
    public float duration;

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
        Debug.Log("applying poison");

        GameObject target = GameObject.Find("GameManager").GetComponent<TargetManager>().GetTarget;

        if (target != null)
        {
            target.GetComponent<EffectSystem>().TakeStatusEffect(id, "speedMod", speedMod, duration);
            target.GetComponent<EffectSystem>().TakeStatusEffect(id, "healthMod", -poisonDamage, duration);
        }
    }
}
