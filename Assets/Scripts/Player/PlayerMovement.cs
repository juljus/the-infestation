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

    void Start()
    {
        currentSpeed = speed;
    }

    public void Movement(InputAction.CallbackContext callbackContext) {
        Vector2 movementInput = callbackContext.ReadValue<Vector2>();
        rigidBody.velocity = new Vector2(movementInput.x * currentSpeed, movementInput.y * currentSpeed);
    }

    // Getters
    public float GetSpeed
    {
        get{ return speed; }
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
