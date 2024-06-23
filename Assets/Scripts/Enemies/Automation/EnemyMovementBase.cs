using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementBase : ScriptableObject
{
    public float aggroRange;
    public float deaggroRange;
    public float stoppingDistance;
    public float speed;

    public virtual void Move(Transform target, Rigidbody2D rigidBody, float playerDistance) {}
}
