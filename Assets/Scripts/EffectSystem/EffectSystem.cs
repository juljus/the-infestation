using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    public StatusEffect[] statusEffectList = new StatusEffect[100];


    public class StatusEffect
    {
        public string id;
        public string type;
        public float value;
        public float duration;
        public float startTime;
        public bool isRemovable = true;
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
            public SpeedModEffect(string id, float value, float duration, UnityEngine.UI.Image icon, bool isRemovable, StatusEffect[] statusEffectList)
            {
                this.id = id;
                this.type = "speedMod";
                this.value = value;
                this.duration = duration;
                this.isRemovable = isRemovable;

                MakeStatusEffect(statusEffectList, icon);
            }
        }

        public class HealthModEffect : StatusEffect
        {
            public HealthModEffect(string id, float value, float duration, UnityEngine.UI.Image icon, bool isRemovable, StatusEffect[] statusEffectList)
            {
                this.id = id;
                this.type = "healthMod";
                this.value = value;
                this.duration = duration;
                this.isRemovable = isRemovable;

                MakeStatusEffect(statusEffectList, icon);
            }
        }

        public class DamageModEffect : StatusEffect
        {
            public DamageModEffect(string id, float value, float duration, UnityEngine.UI.Image icon, bool isRemovable, StatusEffect[] statusEffectList)
            {
                this.id = id;
                this.type = "damageMod";
                this.value = value;
                this.duration = duration;
                this.isRemovable = isRemovable;

                MakeStatusEffect(statusEffectList, icon);
            }
        }
    }


    // PUBLIC FUNCTIONS
    public void TakeStatusEffect(string id, string type, float value, float duration, UnityEngine.UI.Image icon = null, bool isStackable = true, bool isRemovable = true, bool hasDuration = true)
    {
        switch (type)
        {
            case "healthMod":
                TakeStatusEffectExtraBefore(id, isStackable);
                StatusEffect.HealthModEffect healthModEffect = new StatusEffect.HealthModEffect(id, value, duration, icon, isRemovable, statusEffectList);
                TakeStatusEffectExtraAfter(healthModEffect, hasDuration);
                break;

            case "speedMod":
                TakeStatusEffectExtraBefore(id, isStackable);
                StatusEffect.SpeedModEffect speedModEffect = new StatusEffect.SpeedModEffect(id, value, duration, icon, isRemovable, statusEffectList);
                TakeStatusEffectExtraAfter(speedModEffect, hasDuration);
                break;

            case "damageMod":
                TakeStatusEffectExtraBefore(id, isStackable);
                StatusEffect.DamageModEffect damageModEffect = new StatusEffect.DamageModEffect(id, value, duration, icon, isRemovable, statusEffectList);
                TakeStatusEffectExtraAfter(damageModEffect, hasDuration);
                break;

            default:
                break;
        }
    }


    public void RemoveStatusEffectById(string id)
    {
        for (int i = 0; i < statusEffectList.Length; i++)
        {
            if (statusEffectList[i] != null && statusEffectList[i].id == id)
            {
                statusEffectList[i].EndStatusEffect(statusEffectList);
            }
        }
    }
    public void RemoveStatusEffectByTypeAndValue(string type, bool isPositive)
    {
        for (int i = 0; i < statusEffectList.Length; i++)
        {
            if (statusEffectList[i] != null && statusEffectList[i].type == type && statusEffectList[i].isRemovable == true && ((isPositive && statusEffectList[i].value > 1) || (!isPositive && statusEffectList[i].value < 1)))
            {
                statusEffectList[i].EndStatusEffect(statusEffectList);
            }
        }
    }


    // PRIVATE FUNCTIONS
    private void TakeStatusEffectExtraBefore(string id, bool isStackable = true)
    {
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
    }
    private void TakeStatusEffectExtraAfter(StatusEffect effect, bool hasDuration = true)
    {
        if (hasDuration)
        {
            StartCoroutine(statusEffectCorutine(effect));
        }
    }


    // COROUTINES
    private IEnumerator statusEffectCorutine(StatusEffect statusEffect)
    {
        while (Time.time < statusEffect.startTime + statusEffect.duration)
        {
            yield return null;
        }
        statusEffect.EndStatusEffect(statusEffectList);
    }


    // GETTERS
    public StatusEffect[] GetStatusEffectList
    {
        get { return statusEffectList; }
    }
}
