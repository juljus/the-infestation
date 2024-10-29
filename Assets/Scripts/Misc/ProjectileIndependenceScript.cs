using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileIndependenceScript : MonoBehaviour
{
    private Transform parentTransform;
    private Vector3 parentPos;
    private Vector3 parentPosOld;

    private Vector3 parentMoveVector;

    void Start()
    {
        parentTransform = transform.parent.gameObject.transform;
        parentPos = parentTransform.position;
        parentPosOld = parentPos;
    }

    void Update()
    {
        parentPos = parentTransform.position;
        if (parentPos != parentPosOld)
        {            
            parentMoveVector = parentPos - parentPosOld;

            transform.position = transform.position - parentMoveVector;

            parentPosOld = parentPos;
        }
    }
}
