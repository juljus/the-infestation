using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatusEffectManager : MonoBehaviour
{
    private Vector2 canvasStartPosition;
    private float statusEffectIconStep = 60f;
    private EffectSystem.StatusEffect[] statusEffectList;

    void Start()
    {
        // set the canvas start position
        canvasStartPosition = new Vector2(-500f, 225f);
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

        // get current stats
        float maxSpeed = playerMovement.GetMaxSpeed();
        float currentHealth = playerHealth.GetCurrentHealth();

        // get new values
        float[] recieveValues = new float[2];
        UsedFunctions usedFunctions = new UsedFunctions();
        recieveValues = usedFunctions.SetStatsAccordingToStatusEffects(statusEffectList, maxSpeed, currentHealth);

        // set new values
        playerMovement.SetSpeed(recieveValues[0]);
        playerHealth.SetCurrentHealth(recieveValues[1]);
    }

    private void ArrangeStatusEffectIcons()
    {
        int j = 0;
        for (int i = 0; i < statusEffectList.Length; i++)
        {
            if (statusEffectList[i] != null && statusEffectList[i].icon != null)
            {
                statusEffectList[i].icon.GetComponent<RectTransform>().anchoredPosition =  new Vector2(canvasStartPosition.x + (statusEffectIconStep * j), canvasStartPosition.y);
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
