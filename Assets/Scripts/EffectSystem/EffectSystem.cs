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
        if (Input.GetKeyDown(KeyCode.D))
        {
            print("Remove all slow effects");
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
        public string id;
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

                    // instanciate a child element for the statusEffectList[i].icon
                    Instantiate(this.icon, this.icon.transform.position, this.icon.transform.rotation, this.icon.transform);

                    // change the tint of the parent element
                    this.icon.color = new Color(0.5f, 0.5f, 0.5f, 1f);
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
            public SpeedModEffect(string id, float value, float duration, UnityEngine.UI.Image icon, StatusEffect[] statusEffectList)
            {
                this.id = id;
                this.type = "speedMod";
                this.value = value;
                this.duration = duration;

                MakeStatusEffect(statusEffectList, icon);
            }
        }

        public class HealthModEffect : StatusEffect
        {
            public HealthModEffect(string id, float value, float duration, UnityEngine.UI.Image icon, StatusEffect[] statusEffectList)
            {
                this.id = id;
                this.type = "healthMod";
                this.value = value;
                this.duration = duration;

                MakeStatusEffect(statusEffectList, icon);
            }
        }
    }

    public void TakeStatusEffect(string id, string type, float value, float duration, UnityEngine.UI.Image icon = null, bool isStackable = true, bool isRemovable = true, bool hasDuration = true)
    {
        switch (type)
        {
            case "healthMod":
                if (isStackable == false)
                {
                    // check if id is already present in statuseffectlist
                    for (int i = 0; i < statusEffectList.Length; i++)
                    {
                        if (statusEffectList[i] != null && statusEffectList[i].id == id)
                        {
                            statusEffectList[i].EndStatusEffect(statusEffectList);
                        }
                    }
                }

                StatusEffect.HealthModEffect healthModEffect = new StatusEffect.HealthModEffect(id, value, duration, icon, statusEffectList);

                if (hasDuration)
                {
                    StartCoroutine(statusEffectCorutine(healthModEffect));
                }
                break;

            case "speedMod":
                if (isStackable == false)
                {
                    // check if id is already present in statuseffectlist
                    for (int i = 0; i < statusEffectList.Length; i++)
                    {
                        if (statusEffectList[i] != null && statusEffectList[i].id == id)
                        {
                            statusEffectList[i].EndStatusEffect(statusEffectList);
                        }
                    }
                }

                StatusEffect.SpeedModEffect speedModEffect = new StatusEffect.SpeedModEffect(id, value, duration, icon, statusEffectList);
                
                if (hasDuration)
                {
                    StartCoroutine(statusEffectCorutine(speedModEffect));
                }
                break;

            default:
                break;
        }
    }

    public void RemoveStatusEffect(string id)
    {
        for (int i = 0; i < statusEffectList.Length; i++)
        {
            if (statusEffectList[i] != null && statusEffectList[i].id == id)
            {
                statusEffectList[i].EndStatusEffect(statusEffectList);
            }
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
