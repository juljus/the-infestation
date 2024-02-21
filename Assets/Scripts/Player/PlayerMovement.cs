using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody2D rigidBody;

    public void Movement(InputAction.CallbackContext callbackContext) {
        Vector2 movementInput = callbackContext.ReadValue<Vector2>();
        rigidBody.velocity = new Vector2(movementInput.x * speed, movementInput.y * speed);
    }

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }
    public float GetSpeed() {
        return speed;
    }
}
