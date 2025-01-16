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
    // *ABOUT: Moves the player quickly forward in the current facing direction

    public float dashRange;

    private bool isUpgraded = false;

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {
        // get the player's rigidbody
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        // get the player's movement script
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        // start the dash
        skillHelper.StartCoroutine(AbilityCoroutine(rb, playerMovement, skillHelper));
    }

    private IEnumerator AbilityCoroutine(Rigidbody2D rb, PlayerMovement playerMovement, SkillHelper skillHelper)
    {
        float timeElapsed = 0;
        
        Vector2 facingDirection = playerMovement.GetFacingDirection;

        while (timeElapsed < activeTime)
        {
            // move in the facing direction
            rb.velocity = facingDirection * dashRange / activeTime;

            timeElapsed += Time.fixedDeltaTime;
            yield return null;
        }

        // stop the dash
        rb.velocity = Vector2.zero;

        // if upgraded give a shield
        if (isUpgraded)
        {
            skillHelper.StartCoroutine(GiveShield());
        }
    }
    
    // TODO: doesnt do anything yet
    private IEnumerator GiveShield() 
    {
        yield return null;
    }

    //! SETTERS
    public void SetIsUpgraded(bool val)
    {
        isUpgraded = val;
    }
}
