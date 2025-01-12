using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Warrior/(9)Fervor")]
public class FervorSkill : Skill
{
    public float attackTimeMod;
    public float speedMod;
    public float buffDuration;

    public UnityEngine.UI.Image effectIcon;

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {
        // add player attack event listener
        player.GetComponent<PlayerAttack>().playerAttackEvent.AddListener(BuffPlayerFervor);
    }

    public override void Deactivate(GameObject player)
    {
        // remove player attack event listener
        player.GetComponent<PlayerAttack>().playerAttackEvent.RemoveListener(BuffPlayerFervor);
    }

    private void BuffPlayerFervor()
    {
        GameObject player = GameObject.Find("GameManager").GetComponent<PlayerManager>().GetPlayer;

        // get parent effect system
        EffectSystem effectSystem = player.GetComponent<EffectSystem>();

        // apply fervor effect (only one effect will be given an icon so it seems like the they are one effect)
        effectSystem.TakeStatusEffect(id, "attackTimeMod", attackTimeMod, buffDuration, effectIcon);
        effectSystem.TakeStatusEffect(id, "speedMod", speedMod, buffDuration);
    }
}
