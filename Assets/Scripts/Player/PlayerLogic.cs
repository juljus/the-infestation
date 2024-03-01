using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private PlayerScriptableObject playerScriptableObject;
    private GameObject player;
    void Start()
    {
        // get player stuff
        GameObject gameManager = GameObject.Find("GameManager");
        player = gameManager.GetComponent<PlayerManager>().GetPlayer;
        playerScriptableObject = gameManager.GetComponent<PlayerManager>().GetPlayerScriptableObject;

        // instantiate player sprite
        GameObject playerSprite = Instantiate(playerScriptableObject.playerSprite, player.transform, true);
        playerSprite.transform.localScale = new Vector3(1, 1, 1);
    }
}
