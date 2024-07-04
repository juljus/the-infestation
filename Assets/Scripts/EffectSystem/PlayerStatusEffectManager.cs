using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatusEffectManager : MonoBehaviour
{
    private Vector2 effectCanvasStartPosition;
    private float statusEffectIconStep = 110f;
    private EffectSystem.StatusEffect[] statusEffectList;

    void Start()
    {
        // set the canvas start position
        effectCanvasStartPosition = new Vector2(-1199f, 370f);
    }

    void Update()
    {
        // get the status effect array from effect system script
        statusEffectList = transform.GetComponent<EffectSystem>().GetStatusEffectList;

        // move all the null elements to the end of the list
        statusEffectList = statusEffectList.ToList().OrderBy(x => x != null).ToArray();

        // set player stats according to status effects
        SetPlayerStats();

        // arrange status effect icons on screen
        ArrangeStatusEffectIcons();

        // change icon fill amount based on time remaining
        ChangeIconFillAmount();
    }

    private void SetPlayerStats()
    {
        // get scripts
        PlayerMovement playerMovement = transform.GetComponent<PlayerMovement>();
        PlayerHealth playerHealth = transform.GetComponent<PlayerHealth>();
        PlayerAttack playerAttack = transform.GetComponent<PlayerAttack>();

        // get current stats
        float maxSpeed = playerMovement.GetMaxSpeed;
        float currentHealth = playerHealth.GetCurrentHealth;
        float maxAttackDamage = playerAttack.GetMaxAttackDamage;

        // get new values
        float[] recieveValues = new float[3];
        UsedFunctions usedFunctions = new UsedFunctions();
        recieveValues = usedFunctions.SetStatsAccordingToStatusEffects(statusEffectList, maxSpeed, currentHealth, maxAttackDamage);

        // set new values
        playerMovement.SetSpeed(recieveValues[0]);
        playerHealth.SetCurrentHealth(recieveValues[1]);
        playerAttack.SetAttackDamage(recieveValues[2]); 
    }

    private void ArrangeStatusEffectIcons()
    {
        int j = 0;
        for (int i = 0; i < statusEffectList.Length; i++)
        {
            if (statusEffectList[i] != null && statusEffectList[i].icon != null)
            {
                statusEffectList[i].icon.GetComponent<RectTransform>().anchoredPosition =  new Vector2(effectCanvasStartPosition.x + (statusEffectIconStep * j), effectCanvasStartPosition.y);
                j += 1;
            }
        }
    }

    private void ChangeIconFillAmount()
    {
        for (int i = 0; i < statusEffectList.Length; i++)
        {
            if (statusEffectList[i] != null && statusEffectList[i].icon != null)
            {
                statusEffectList[i].icon.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 1 - ((Time.time - statusEffectList[i].startTime) / statusEffectList[i].duration);
            }
        }
    }
}
