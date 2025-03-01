using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShacklesScript : MonoBehaviour
{
    private float flyTime;
    private GameObject target;
    private float rotationsPerSecond;

    private float timeLeft;

    // Update is called once per frame
    void Update()
    {
        if (flyTime <= 0)
        {
            // move the shackles to the target
            if (target != null)
            {
                transform.position = target.transform.position;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // calculate speed
        float shacklesSpeed = Vector2.Distance(transform.position, target.transform.position) / flyTime;
        flyTime -= Time.deltaTime;

        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, shacklesSpeed * Time.deltaTime);

            // rotate the dagger at the speed of x rotations per second
            transform.Rotate(0, 0, 360 * rotationsPerSecond * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == target)
        {
            Destroy(gameObject);
        }
    }

    // SETTERS
    public void SetFlyTime(float time)
    {
        this.flyTime = time;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void SetRotationsPerSecond(float rotationsPerSecond)
    {
        this.rotationsPerSecond = rotationsPerSecond;
    }
}
