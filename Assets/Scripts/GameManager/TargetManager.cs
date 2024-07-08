using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private GameObject target;

    public GameObject GetTarget {
        get { return target; }
    }

    public void SetTarget(GameObject newTarget) {
        target = newTarget;
    }

    public void ClearTarget() {
        target = null;
    }

    public void TargetClosestEnemy() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies) {
            float distance = Vector2.Distance(GetComponent<PlayerManager>().GetPlayerTransform.position, enemy.transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        target = closestEnemy;
    }

}
