using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(menuName = "Skills/Warrior/(0)Dash")]
public class DashSkill : Skill
{
    // ABOUT: Moves the player quickly forward in the current facing direction

    public float dashRange;

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {
        // get the player's rigidbody
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        // get the player's movement script
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        // start the dash
        skillHelper.StartCoroutine(AbilityCoroutine(rb, playerMovement));
    }

    private IEnumerator AbilityCoroutine(Rigidbody2D rb, PlayerMovement playerMovement)
    {
        float timeElapsed = 0;
        
        while (timeElapsed < activeTime)
        {
            // FIX: facing direction is always 0,0
            Vector2 facingDirection = playerMovement.GetFacingDirection();
            
            rb.transform.position = Vector2.MoveTowards(rb.transform.position, facingDirection, dashRange/activeTime * Time.deltaTime);

            timeElapsed += Time.fixedDeltaTime;
            yield return null;
        }
    }
}
