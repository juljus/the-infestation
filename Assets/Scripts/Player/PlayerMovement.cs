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

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject sprite;

    private float speed;

    private Vector2 facingDirection;
    private bool directionRight;
    private bool isMoving;

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

            if (movementInput.x > 0)
            {
                directionRight = true;
                // x rotation to 0
                sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                directionRight = false;
                // x rotation to 180
                sprite.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        rigidBody.velocity = new Vector2(movementInput.x * currentSpeed, movementInput.y * currentSpeed);

        animator.SetBool("isMoving", isMoving);
        animator.SetBool("directionRight", directionRight);
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

    //! IDataPersistance
    public void InGameSave(ref GameData data)
    {
    }

    public void LoadData(GameData data)
    {
        int selectedCharacter = data.selectedChar;

        this.speed = data.playerMovementSpeed[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
    }
}
