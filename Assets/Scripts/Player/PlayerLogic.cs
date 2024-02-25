using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] private Vector2 canvasStartPosition;
    [SerializeField] private float statusEffectIconStep = 75f;

    // static variables
    static GameObject thisGameObject;
    static StatusEffect[] statusEffectList = new StatusEffect[15];

    void Start()
    {
        canvasStartPosition = new Vector2(-500f, 265f);

        thisGameObject = gameObject;
    }

    void Update()
    {
        // move all the null elements to the end of the list
        statusEffectList = statusEffectList.OrderBy(x => x != null).ToArray();

        // arrange status effect icons on screen
        int j = 0;
        for (int i = 0; i < statusEffectList.Length; i++)
        {
            if (statusEffectList[i] != null)
            {
                statusEffectList[i].icon.GetComponent<RectTransform>().anchoredPosition =  new Vector2(canvasStartPosition.x + (statusEffectIconStep * j), canvasStartPosition.y);
                j += 1;
            }
        }

        // when space is pressed, remove all speed slow effects
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < statusEffectList.Length; i++)
            {
                if (statusEffectList[i] != null && statusEffectList[i].type == "speedMod" && statusEffectList[i].value < 1)
                {
                    StatusEffect.SpeedModEffect speedModEffect = statusEffectList[i] as StatusEffect.SpeedModEffect;

                    speedModEffect.EndEffect();
                }
            }
        }
    }

    class StatusEffect
    {
        public string type;
        public float value;
        public float duration;
        public float startTime;
        public UnityEngine.UI.Image icon;

        public class SpeedModEffect : StatusEffect
        {
            public SpeedModEffect(float value, float duration, UnityEngine.UI.Image icon)
            {
                this.type = "speedMod";
                this.value = value;
                this.duration = duration;
                this.icon = Instantiate(icon, new Vector3(-1000, -1000, 0), Quaternion.identity, GameObject.Find("MainCanvas").transform);
                this.startTime = Time.time;

                for (int i = 0; i < statusEffectList.Length; i++)
                {
                    if (statusEffectList[i] == null)
                    {
                        statusEffectList[i] = this;
                        break;
                    }
                }
            }
            public void StartEffect()
            {
                float speed = thisGameObject.GetComponent<PlayerMovement>().GetSpeed();
                thisGameObject.GetComponent<PlayerMovement>().SetSpeed(speed * this.value);
            }

            public void EndEffect()
            {
                float speed = thisGameObject.GetComponent<PlayerMovement>().GetSpeed();
                thisGameObject.GetComponent<PlayerMovement>().SetSpeed(speed / this.value);

                // remove status effect icon
                Destroy(this.icon.gameObject);

                // replace status effect in list with null
                for (int i = 0; i < statusEffectList.Length; i++)
                {
                    if (statusEffectList[i] == this)
                    {
                        statusEffectList[i] = null;
                        break;
                    }
                }
            }
        }
    }

    public void TakeEffect(string type, float value, float duration, UnityEngine.UI.Image icon = null)
    {
        switch (type)
        {
            case "damage":
                GetComponent<PlayerHeath>().TakeDamage(value);
                break;
            case "heal":
                GetComponent<PlayerHeath>().Heal(value);
                break;
            case "speedMod":
                StatusEffect.SpeedModEffect speedModEffect = new StatusEffect.SpeedModEffect(value, duration, icon);
                StartCoroutine(speedModEffectCorutine(speedModEffect));
                break;
            default:
                break;
        }
    }

    private IEnumerator speedModEffectCorutine(StatusEffect.SpeedModEffect speedMod)
    {
        speedMod.StartEffect();
        while (Time.time < speedMod.startTime + speedMod.duration)
        {
            // check if the effect was removed
            if (speedMod.icon == null)
            {
                yield break;
            }

            speedMod.icon.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().fillAmount = 1 - ((Time.time - speedMod.startTime) / speedMod.duration);
            yield return null;
        }
        speedMod.EndEffect();
    }
}
