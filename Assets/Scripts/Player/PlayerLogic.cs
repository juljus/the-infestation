using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private PlayerScriptableObject playerScriptableObject;

    private int isStunned;

    [SerializeField] private Animator playerAnimator;
    
    void Start()
    {
        // get player stuff
        GameObject gameManager = GameObject.Find("GameManager");
        playerScriptableObject = gameManager.GetComponent<PlayerManager>().GetPlayerScriptableObject;

        // // instantiate player sprite
        // GameObject playerSprite = Instantiate(playerScriptableObject.playerSprite, gameObject.transform, true);
        // playerSprite.transform.localScale = new Vector3(1, 1, 1);
    }

    // GETTERS
    public int GetIsStunned { get { return isStunned; } }

    // SETTERS
    public void Stun() { isStunned ++; }
    public void UnStun() { isStunned --; }

    public void SetIfThrowing(bool isThrowing)
    {
        playerAnimator.SetBool("isThrowing", isThrowing);
    }
}
