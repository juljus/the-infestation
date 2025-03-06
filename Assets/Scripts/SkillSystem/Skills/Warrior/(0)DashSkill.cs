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
    public float shieldAmount;

    private bool isUpgraded = false;

    public override void Activate(GameObject player, SkillHelper skillHelper)
    {
        // get the player's rigidbody
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        // get the player's movement script
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        // start the dash
        skillHelper.StartCoroutine(AbilityCoroutine(rb, playerMovement, player));
    }

    private IEnumerator AbilityCoroutine(Rigidbody2D rb, PlayerMovement playerMovement, GameObject player)
    {
        float timeElapsed = 0;
        
        Vector2 facingDirection = playerMovement.GetFacingDirection;

        while (timeElapsed < activeTime)
        {
            // move in the facing direction
            rb.linearVelocity = facingDirection * dashRange / activeTime;

            timeElapsed += Time.fixedDeltaTime;
            yield return null;
        }

        // stop the dash
        rb.linearVelocity = Vector2.zero;

        // if upgraded give a shield
        if (isUpgraded)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.SetShield(shieldAmount);
        }
    }
    
    //! setters
    public override void SetIsUpgraded(bool val)
    {
        isUpgraded = val;
    }
}
