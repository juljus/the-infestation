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

    private float[] charCoords;

    void Start()
    {
        currentSpeed = speed;

        // place player on the map
        transform.position = new Vector3(charCoords[0], charCoords[1], 0);
    }

    public void Movement(InputAction.CallbackContext callbackContext)
    {
        if (transform.GetComponent<PlayerLogic>().GetIsStunned > 0) { return; }
        if (transform.GetComponent<PlayerAttack>().GetIsAttacking) { return; }

        Vector2 movementInput = callbackContext.ReadValue<Vector2>();

        if (movementInput != Vector2.zero)
        {
            facingDirection = movementInput.normalized;

            if (movementInput.x > 0)
            {
                directionRight = true;

                sprite.transform.localScale = new Vector3(1, 1, 1);
                sprite.transform.GetChild(0).localScale = new Vector3(1, 1, 1);
            }
            else
            {
                directionRight = false;

                sprite.transform.localScale = new Vector3(-1, 1, 1);
                sprite.transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
            }

            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        rigidBody.linearVelocity = new Vector2(movementInput.x * currentSpeed, movementInput.y * currentSpeed);

        animator.SetBool("isMoving", isMoving);
    }

    // PUBLIC FUNCTIONS
    public void FaceTarget(GameObject target)
    {
        Vector2 targetPosition = target.transform.position;
        Vector2 direction = targetPosition - (Vector2)transform.position;

        facingDirection = direction.normalized;

        if (direction.x > 0)
        {
            directionRight = true;

            sprite.transform.localScale = new Vector3(1, 1, 1);
            sprite.transform.GetChild(0).localScale = new Vector3(1, 1, 1);
        }
        else
        {
            directionRight = false;

            sprite.transform.localScale = new Vector3(-1, 1, 1);
            sprite.transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
        }
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
        int selectedCharacter = data.selectedChar;

        data.charCoords[selectedCharacter][0] = transform.position.x;
        data.charCoords[selectedCharacter][1] = transform.position.y;
    }

    public void LoadData(GameData data)
    {
        int selectedCharacter = data.selectedChar;

        this.speed = data.playerMovementSpeed[selectedCharacter];
        this.charCoords = data.charCoords[selectedCharacter];
    }

    public void SaveData(ref GameData data)
    {
    }
}
