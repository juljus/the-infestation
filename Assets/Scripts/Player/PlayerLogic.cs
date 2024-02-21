using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] private Vector3 canvasStartPosition;
    [SerializeField] private float statusEffectIconStep = 75f;

    static UnityEngine.UI.Image[] statusEffectIconList = new UnityEngine.UI.Image[15];

    void Update()
    {
        // print(statusEffectIcons);

        for (int i = 0; i < statusEffectIconList.Length; i++)
        {
            if (statusEffectIconList[i] != null)
            {
                statusEffectIconList[i].transform.position = new Vector3(canvasStartPosition.x + (statusEffectIconStep * i), canvasStartPosition.y, canvasStartPosition.z);
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

                for (int i = 0; i < statusEffectIconList.Length; i++)
                {
                    if (statusEffectIconList[i] == null)
                    {
                        statusEffectIconList[i] = this.icon;
                        break;
                    }
                }
            }
        }

        // public StatusEffect(string type, float value, float duration, UnityEngine.UI.Image icon)
        // {
        //     this.type = type;
        //     this.value = value;
        //     this.duration = duration;
        //     this.icon = Instantiate(icon, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("MainCanvas").transform);
        //     this.startTime = Time.time;
        // }
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
        float speed = GetComponent<PlayerMovement>().GetSpeed();
        GetComponent<PlayerMovement>().SetSpeed(speed * speedMod.value);

        while (Time.time < speedMod.startTime + speedMod.duration)
        {
            speedMod.icon.fillAmount = 1 - ((Time.time - speedMod.startTime) / speedMod.duration);
            yield return null;
        }

        speed = GetComponent<PlayerMovement>().GetSpeed();
        GetComponent<PlayerMovement>().SetSpeed(speed / speedMod.value);
        Destroy(speedMod.icon.gameObject);
    }
}
