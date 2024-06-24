using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private PlayerScriptableObject playerScriptableObject;
    
    void Start()
    {
        // get player stuff
        GameObject gameManager = GameObject.Find("GameManager");
        playerScriptableObject = gameManager.GetComponent<PlayerManager>().GetPlayerScriptableObject;

        // instantiate player sprite
        GameObject playerSprite = Instantiate(playerScriptableObject.playerSprite, gameObject.transform, true);
        playerSprite.transform.localScale = new Vector3(1, 1, 1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // deal damage to enemy
            GameObject enemy = GameObject.Find("Enemy");
            enemy.GetComponent<EnemyHealth>().TakeDamage(10);
        }
    }
}
