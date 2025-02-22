using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private GameObject target;

    public GameObject GetTarget {
        get { return target; }
    }

    public GameObject GetTargetSmart() {
        if (target == null) {
            TargetClosestEnemy();
        }

        return target;
    }

    public void SetTarget(GameObject newTarget) {
        if (target != null) {
            target.GetComponent<Target>().ClearTarget();
        }
        target = newTarget;
        target.GetComponent<Target>().SetTarget();
    }

    public void ClearTarget() {
        target.GetComponent<Target>().ClearTarget();
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
        target.GetComponent<Target>().SetTarget();
    }
    
}
