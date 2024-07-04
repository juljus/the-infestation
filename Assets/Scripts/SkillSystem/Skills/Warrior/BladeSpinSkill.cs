using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Warrior/BladeSpin")]
public class BladeSpinSkill : Skill
{
    public float range;
    public float bonusDamage;
    public float attackDamageModifier;

    private float dealDamage;
    private Collider2D[] hitEnemies;

    // spins your sword around you, dealing damage to all enemies in range based on your attack damage

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {   
        PlayerAttack playerAttackScript = player.GetComponent<PlayerAttack>();

        dealDamage = playerAttackScript.GetAttackDamage * attackDamageModifier + bonusDamage;

        skillHelper.StartCoroutine(HitEnemies(player));
    }

    private IEnumerator HitEnemies(GameObject player)
    {
        // draw a red circle around the player
        Debug.DrawLine(player.transform.position, new Vector3(player.transform.position.x + range, player.transform.position.y, player.transform.position.z), Color.red, activeTime);

        yield return new WaitForSeconds(activeTime);

        hitEnemies = Physics2D.OverlapCircleAll(player.transform.position, range);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(dealDamage);
            }
        }

        // delete the circle
        Debug.DrawLine(player.transform.position, new Vector3(player.transform.position.x + range, player.transform.position.y, player.transform.position.z), Color.clear, 0);
    }
}
