using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// *ABOUT: upgrade dash - dashing gives a small damage blocking shield and the cooldown of dash is refreshed when an enemy is killed.

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

        // upgrade dash
        player.GetComponent<PlayerSkillHolder>().GetSkillById(0).SetIsUpgraded(true);
    }

    public override void Deactivate(GameObject player)
    {
        // remove listener
        GameObject.Find("GameManager").GetComponent<EnemyLogicManager>().enemyDeathEvent.RemoveListener(Refresh);
    }

    private void Refresh()
    {
        player.GetComponent<PlayerSkillHolder>().SkipSkill0Cooldown();
    }
}
