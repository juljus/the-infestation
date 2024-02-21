using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLogicOld : MonoBehaviour
{
    [SerializeField] private Vector3 canvasStartPosition;
    [SerializeField] private float statusEffectIconStep = 75f;
    [SerializeField] private UnityEngine.UI.Image statusEffectIcon;

    private UnityEngine.UI.Image[] statusEffectIcons = new UnityEngine.UI.Image[15];
    void Start()
    {
        canvasStartPosition = new Vector3(-536.5f, 301.75f, 0f);
    }

    void Update()
    {
        // print(statusEffectIcons);

        for (int i = 0; i < statusEffectIcons.Length; i++)
        {
            if (statusEffectIcons[i] != null)
            {
                statusEffectIcons[i].transform.position = new Vector3(canvasStartPosition.x + (statusEffectIconStep * i), canvasStartPosition.y, canvasStartPosition.z);
            }
        }
    }

    public void TakeEffect(string type, float value, float duration)
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
                StartCoroutine(speed(duration, value));
                break;
            case "slow":
                StartCoroutine(slow(duration, value));
                break;
            default:
                break;
        }
    }

    private IEnumerator speed(float duration, float value)
    {
        float speed = GetComponent<PlayerMovement>().GetSpeed();
        GetComponent<PlayerMovement>().SetSpeed(speed * value);

        UnityEngine.UI.Image statusEffectIconClone = Instantiate(statusEffectIcon, new Vector3(0, 0, 0), Quaternion.identity, transform);
        statusEffectIconClone.fillAmount = 1;
        statusEffectIconClone.transform.SetParent(GameObject.Find("MainCanvas").transform, false);
        
        for (int i = 0; i < statusEffectIcons.Length; i++)
        {
            if (statusEffectIcons[i] == null)
            {
                statusEffectIcons[i] = statusEffectIconClone;
                break;
            }
        }

        float speedTimeRemaining = duration;
        float speedDuration = duration;

        while (speedTimeRemaining > 0)
        {
            speedTimeRemaining -= Time.deltaTime;
            statusEffectIconClone.fillAmount = speedTimeRemaining / speedDuration;
            yield return null;
        }

        speed = GetComponent<PlayerMovement>().GetSpeed();
        GetComponent<PlayerMovement>().SetSpeed(speed / value);
        Destroy(statusEffectIconClone.gameObject);
    }
    private IEnumerator slow(float duration, float value)
    {
        float speed = GetComponent<PlayerMovement>().GetSpeed();
        GetComponent<PlayerMovement>().SetSpeed(speed * value);

        UnityEngine.UI.Image statusEffectIconClone = Instantiate(statusEffectIcon, new Vector3(0, 0, 0), Quaternion.identity, transform);
        statusEffectIconClone.fillAmount = 1;
        statusEffectIconClone.transform.SetParent(GameObject.Find("MainCanvas").transform, false);

        for (int i = 0; i < statusEffectIcons.Length; i++)
        {
            if (statusEffectIcons[i] == null)
            {
                statusEffectIcons[i] = statusEffectIconClone;
                break;
            }
        }

        float slowTimeRemaining = duration;
        float slowDuration = duration;

        while (slowTimeRemaining > 0)
        {
            slowTimeRemaining -= Time.deltaTime;
            statusEffectIconClone.fillAmount = slowTimeRemaining / slowDuration;
            yield return null;
        }

        speed = GetComponent<PlayerMovement>().GetSpeed();
        GetComponent<PlayerMovement>().SetSpeed(speed / value);
        Destroy(statusEffectIconClone.gameObject);
    }

}
