using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float maxSpeed;
    private float speed;

    void Start()
    {
        speed = maxSpeed;
    }

    // Getters 

    // effect1
    public float GetMaxSpeed() {
        return maxSpeed;
    }

    // Setters
    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }
}