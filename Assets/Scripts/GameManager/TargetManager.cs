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

}
