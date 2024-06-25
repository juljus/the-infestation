using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementBase : ScriptableObject
{
    // ---------- EDITOR STATS ----------
    public float aggroRange;
    public float deaggroRange;
    public float stoppingDistance;
    public float speed;


    // ---------- VARIABLES ----------
    [HideInInspector] public float currentSpeed;


    // ---------- GETTERS ----------
    public float GetSpeed { get { return speed; } }
    public float GetCurrentSpeed { get { return currentSpeed; } }


    // ---------- SETTERS ----------
    public void SetSpeed(float newSpeed) { currentSpeed = newSpeed; }


    // ---------- VIRTUAL FUNCTIONS ----------
    public virtual void Move(Transform target, Rigidbody2D rigidBody, float playerDistance) {}
}
