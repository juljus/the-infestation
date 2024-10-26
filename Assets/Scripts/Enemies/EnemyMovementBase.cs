using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement")]
public class EnemyMovementBase : ScriptableObject
{
    // ---------- EDITOR STATS ----------
    public float aggroRange;
    public float deaggroRange;
    public float stoppingDistance;
    public float speed;


    // ---------- VARIABLES ----------
    [HideInInspector] public float currentSpeed;

    [HideInInspector] public GameObject target;


    // ---------- GETTERS ----------
    public float GetSpeed { get { return speed; } }
    public float GetCurrentSpeed { get { return currentSpeed; } }


    // ---------- SETTERS ----------
    public void SetSpeed(float newSpeed) { currentSpeed = newSpeed; }
    public void SetMaxSpeed(float newMaxSpeed) { speed = newMaxSpeed; }


    // ---------- VIRTUAL FUNCTIONS ----------
    public virtual void Move(Transform target, Rigidbody2D rigidBody, float playerDistance) {}
    
    public virtual EnemyMovementBase Clone() { return null; }
}
