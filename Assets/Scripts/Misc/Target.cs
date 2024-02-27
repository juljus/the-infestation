using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private void OnMouseDown() {
        GameObject gameManager = GameObject.Find("GameManager");
        gameManager.GetComponent<TargetManager>().SetTarget(gameObject);
    }
}
