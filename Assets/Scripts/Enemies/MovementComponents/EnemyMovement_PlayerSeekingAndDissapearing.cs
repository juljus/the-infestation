using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement", menuName = "Enemy/Movement/PlayerSeekingAndDissapearing")]
public class EnemyMovement_PlayerSeekingAndDissapearing : EnemyMovementBase
{
    // this can NOT be changed in the editor
    private float dissapearCooldown = 10f;
    private float dissapearTime = 2f;
    private float dissapearMinDistance = 3f;
    // -------------------------------------

    private float dissapearCooldownRemaining;
    private bool isDissapearing = false;
    private bool isDissapearingCooldown = false;

    private bool isAggroed = false;

    public override void Move(Transform player, Rigidbody2D rigidBody, float playerDistance)
    {   
        target = player.gameObject;

        if (playerDistance <= aggroRange)
        {
            isAggroed = true;
        }
        else if (playerDistance >= deaggroRange)
        {
            isAggroed = false;
        }

        dissapearCooldownRemaining -= Time.deltaTime;

        if (isAggroed && playerDistance >= dissapearMinDistance && dissapearCooldownRemaining <= 0)
        {
            GameObject.Find("GameManager").GetComponent<SkillHelper>().StartCoroutine(Dissapear(rigidBody.gameObject, player));

            dissapearCooldownRemaining = dissapearCooldown;
        }
        else if (isAggroed && playerDistance > stoppingDistance)
        {
            rigidBody.position = Vector2.MoveTowards(rigidBody.position, target.transform.position, currentSpeed * Time.deltaTime);
        }
    }

    public override EnemyMovementBase Clone()
    {
        var clone = ScriptableObject.CreateInstance<EnemyMovement_PlayerSeekingAndDissapearing>();

        // copy over editor stats
        clone.aggroRange = aggroRange;
        clone.deaggroRange = deaggroRange;
        clone.stoppingDistance = stoppingDistance;
        clone.speed = speed;

        clone.currentSpeed = speed;

        return clone;
    }

    private IEnumerator Dissapear(GameObject gameObject, Transform player)
    {
        // dissapear
        isDissapearing = true;

        for (int i = 0; i < 3; i++)
        {
            gameObject.SetActive(false);
            yield return new WaitForSeconds(0.03f);
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.03f);
        }
        gameObject.SetActive(false);

        yield return new WaitForSeconds(1);
        gameObject.transform.position = player.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0);

        // appear
        for (int i = 0; i < 3; i++)
        {
            gameObject.SetActive(true);
            yield return new WaitForSeconds(0.03f);
            gameObject.SetActive(false);
            yield return new WaitForSeconds(0.03f);
        }
        gameObject.SetActive(true);

        isDissapearing = false;

        GameObject.Find("GameManager").GetComponent<SkillHelper>().StartCoroutine(DissapearCooldown());
    }

    private IEnumerator DissapearCooldown()
    {
        isDissapearingCooldown = true;
        yield return new WaitForSeconds(dissapearCooldown);
        isDissapearingCooldown = false;
    }
}
