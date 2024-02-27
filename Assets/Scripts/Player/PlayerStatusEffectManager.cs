using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatusEffectManager : MonoBehaviour
{
    private Vector2 canvasStartPosition;
    private float statusEffectIconStep = 75f;
    private EffectSystem.StatusEffect[] statusEffectList;

    void Start()
    {
        // set the canvas start position
        canvasStartPosition = new Vector2(-500f, 265f);
    }

    void Update()
    {
        // get the status effect array from effect system script
        statusEffectList = transform.GetComponent<EffectSystem>().GetStatusEffectList;

        // move all the null elements to the end of the list
        statusEffectList = statusEffectList.ToList().OrderBy(x => x != null).ToArray();
        // print status effect list

        // set player stats according to status effects
        SetPlayerSpeed();

        // arrange status effect icons on screen
        int j = 0;
        for (int i = 0; i < statusEffectList.Length; i++)
        {
            if (statusEffectList[i] != null && statusEffectList[i].icon != null)
            {
                statusEffectList[i].icon.GetComponent<RectTransform>().anchoredPosition =  new Vector2(canvasStartPosition.x + (statusEffectIconStep * j), canvasStartPosition.y);
                j += 1;
            }
        }

        // change icon fill amount based on time remaining
        for (int i = 0; i < statusEffectList.Length; i++)
        {
            if (statusEffectList[i] != null && statusEffectList[i].icon != null)
            {
                statusEffectList[i].icon.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 1 - ((Time.time - statusEffectList[i].startTime) / statusEffectList[i].duration);
            }
        }
    }

    private void SetPlayerSpeed()
    {
        float newSpeed = transform.GetComponent<PlayerMovement>().GetMaxSpeed();
        for (int i = 0; i < statusEffectList.Length; i++)
        {
            EffectSystem.StatusEffect.SpeedModEffect speedModEffect = statusEffectList[i] as EffectSystem.StatusEffect.SpeedModEffect;
            if (speedModEffect != null && speedModEffect.type == "speedMod" && speedModEffect.value < 1f)
            {
                newSpeed *= statusEffectList[i].value;
            }
        }
        transform.GetComponent<PlayerMovement>().SetSpeed(newSpeed);
    }
}
