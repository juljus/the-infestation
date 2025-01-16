using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// *ABOUT: upgrade dash - dashing gives a small damage blocking shield and the cooldown of dash is refreshed when an enemy is killed.

// BUG: this ability buggs out the game, stops saving.... also some error on line 28 about object not existing

[CreateAssetMenu(menuName = "Skills/Warrior/(12)DashRefresh")]
public class DashRefreshSkill : Skill
{
    // public UnityEngine.UI.Image effectIcon;

    private GameObject player;

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {
        this.player = player;

        // add listener
        GameObject.Find("GameManager").GetComponent<EnemyLogicManager>().enemyDeathEvent.AddListener(Refresh);

        // // add icon
        // player.GetComponent<EffectSystem>().TakeStatusEffect(id, "speedMod", 1, 0, effectIcon, false, false, false);

        // upgrade dash
        player.GetComponent<PlayerSkillHolder>().GetSkillById(0).SetIsUpgraded(true);
    }

    public override void Deactivate(GameObject player)
    {
        // remove listener
        GameObject.Find("GameManager").GetComponent<EnemyLogicManager>().enemyDeathEvent.RemoveListener(Refresh);

        // // remove icon
        // player.GetComponent<EffectSystem>().RemoveStatusEffectById(id);
    }

    private void Refresh()
    {
        player.GetComponent<PlayerSkillHolder>().SkipSkill0Cooldown();
    }
}
