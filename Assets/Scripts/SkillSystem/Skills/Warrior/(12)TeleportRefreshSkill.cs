using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Warrior/(12)TeleportRefresh")]
public class TeleportRefreshSkill : Skill
{
    public UnityEngine.UI.Image effectIcon;

    private GameObject player;

    // Passive: after killing an enemy refreshes the cooldown of your teleport skill.

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {
        this.player = player;

        // add listener
        GameObject.Find("GameManager").GetComponent<EnemyLogicManager>().enemyDeathEvent.AddListener(Refresh);

        // add icon
        player.GetComponent<EffectSystem>().TakeStatusEffect(id, "speedMod", 1, 0, effectIcon, false, false, false);
    }

    public override void Deactivate(GameObject player)
    {
        // remove listener
        GameObject.Find("GameManager").GetComponent<EnemyLogicManager>().enemyDeathEvent.RemoveListener(Refresh);

        // remove icon
        player.GetComponent<EffectSystem>().RemoveStatusEffectById(id);
    }

    private void Refresh()
    {
        player.GetComponent<PlayerSkillHolder>().SkipSkill2Cooldown();
    }
}
