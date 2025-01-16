using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IDataPersistance
{
    [SerializeField] private Rigidbody2D rigidBody;

    [SerializeField] private float currentSpeed;

    private float speed;

    private Vector2 facingDirection;

    void Start()
    {
        currentSpeed = speed;
    }

    public void Movement(InputAction.CallbackContext callbackContext)
    {
        if (transform.GetComponent<PlayerLogic>().GetIsStunned > 0) { return; }

        Vector2 movementInput = callbackContext.ReadValue<Vector2>();

        if (movementInput != Vector2.zero)
        {
            facingDirection = movementInput;
        }

        rigidBody.velocity = new Vector2(movementInput.x * currentSpeed, movementInput.y * currentSpeed);
    }

    // Getters
    public float GetSpeed
    {
        get { return speed; }
    }

    public Vector2 GetFacingDirection
    {
        get { return facingDirection; }
    }

    // Setters
    public void SetCurrentSpeed(float newSpeed)
    {
        currentSpeed = newSpeed;
    }

    // IDataPersistance

    public void LoadData(GameData data)
    {
        int selectedCharacter = data.selectedCharacter;

        this.speed = data.playerMovementSpeed[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
        int selectedCharacter = data.selectedCharacter;

        data.playerMovementSpeed[selectedCharacter] = this.speed;
    }
}
