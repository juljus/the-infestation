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
                this.icon = Instantiate(icon, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("MainCanvas").transform);
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
            case "speed":
                StatusEffect.SpeedModEffect speedEffect = new StatusEffect.SpeedModEffect(value, duration, icon);
                StartCoroutine(speedModEffectCorutine(speedEffect));
                break;
            case "slow":
                StatusEffect.SpeedModEffect slowEffect = new StatusEffect.SpeedModEffect(value, duration, icon);                
                StartCoroutine(speedModEffectCorutine(slowEffect));
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
            speedMod.icon.fillAmount = 1 - ((Time.time - speedMod.startTime) / speedMod.duration);
            yield return null;
        }
        speedMod.EndEffect();
    }
}
