using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image tarIndicator;

    private void Start() {
        tarIndicator.gameObject.SetActive(false);
    }

    private void OnMouseDown() {
        GameObject gameManager = GameObject.Find("GameManager");
        gameManager.GetComponent<TargetManager>().SetTarget(gameObject);
    }

    public void ClearTarget() {
        tarIndicator.gameObject.SetActive(false);
    }

    public void SetTarget() {
        tarIndicator.gameObject.SetActive(true);
    }
}
