using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    public StatusEffect[] statusEffectList = new StatusEffect[100];

    void Update()
    {
        // when space is pressed, remove all slow effects
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < statusEffectList.Length; i++)
            {
                if (statusEffectList[i] != null && statusEffectList[i].type == "speedMod" && statusEffectList[i].value < 1)
                {
                    StatusEffect.SpeedModEffect speedModEffect = statusEffectList[i] as StatusEffect.SpeedModEffect;

                    speedModEffect.EndStatusEffect(statusEffectList);
                }
            }
        }
    }

    public class StatusEffect
    {
        public string type;
        public float value;
        public float duration;
        public float startTime;
        public UnityEngine.UI.Image icon;

        public void EndStatusEffect(StatusEffect[] statusEffectList)
        {
            // remove status effect icon
            if (this.icon != null)
            {
                Destroy(this.icon.gameObject);
            }

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

        public void MakeStatusEffect(StatusEffect[] statusEffectList, UnityEngine.UI.Image icon = null)
        {
                if (icon != null)
                {
                    this.icon = Instantiate(icon, new Vector3(-1000, -1000, 0), Quaternion.identity, GameObject.Find("SecondaryCanvas").transform);
                }

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

        public class SpeedModEffect : StatusEffect
        {
            public SpeedModEffect(float value, float duration, UnityEngine.UI.Image icon, StatusEffect[] statusEffectList)
            {
                this.type = "speedMod";
                this.value = value;
                this.duration = duration;

                MakeStatusEffect(statusEffectList, icon);
            }
        }

        public class HealthModEffect : StatusEffect
        {
            public HealthModEffect(float value, float duration, UnityEngine.UI.Image icon, StatusEffect[] statusEffectList)
            {
                this.type = "healthMod";
                this.value = value;
                this.duration = duration;

                MakeStatusEffect(statusEffectList, icon);
            }
        }
    }

    public void TakeStatusEffect(string type, float value, float duration, UnityEngine.UI.Image icon = null)
    {
        switch (type)
        {
            case "healthMod":
                StatusEffect.HealthModEffect healthModEffect = new StatusEffect.HealthModEffect(value, duration, icon, statusEffectList);
                StartCoroutine(statusEffectCorutine(healthModEffect));
                break;
            case "speedMod":
                StatusEffect.SpeedModEffect speedModEffect = new StatusEffect.SpeedModEffect(value, duration, icon, statusEffectList);
                StartCoroutine(statusEffectCorutine(speedModEffect));
                break;
            default:
                break;
        }
    }

    private IEnumerator statusEffectCorutine(StatusEffect statusEffect)
    {
        while (Time.time < statusEffect.startTime + statusEffect.duration)
        {
            yield return null;
        }
        statusEffect.EndStatusEffect(statusEffectList);
    }

    // Getters
    public StatusEffect[] GetStatusEffectList
    {
        get { return statusEffectList; }
    }
}
