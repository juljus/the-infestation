using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    private PlayerScriptableObject playerScriptableObject;

    private int isStunned;
    
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
        // FOR EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject enemy = GameObject.Find("Enemy");
            enemy.GetComponent<EffectSystem>().TakeStatusEffect("odagfpjplo3", "speedMod", 0.2f, 10f);
            print("Enemy slowed by to 20% for 10 seconds");
        }

        // FOR EDITOR
        // when D is pressed, remove all slow effects
        if (Input.GetKeyDown(KeyCode.D))
        {
            print("Remove all slow effects");
            GetComponent<EffectSystem>().RemoveStatusEffectByTypeAndValue("speedMod", false);
        }

    }

    // GETTERS
    public int GetIsStunned { get { return isStunned; } }

    // SETTERS
    public void Stun() { isStunned ++; }
    public void UnStun() { isStunned --; }
}
