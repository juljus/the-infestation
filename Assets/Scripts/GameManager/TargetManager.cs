using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float targetRange;

    private void Update() {
        float distance = Vector2.Distance(GetComponent<PlayerManager>().GetPlayerTransform.position, target.transform.position);
        if (distance > targetRange) {
            ClearTarget();
        }
    }

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
        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        // add with tags boss and minion also to the list
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Boss"));
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Minion"));

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

    public GameObject GetClosestEnemy() {
        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        
        // add with tags boss and minion also to the list
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Boss"));
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Minion"));

        float minDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies) {
            float distance = Vector2.Distance(GetComponent<PlayerManager>().GetPlayerTransform.position, enemy.transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
    
}
