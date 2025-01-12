using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDaggerScript : MonoBehaviour
{
    private float daggerSpeed;
    private float daggerDamage;
    private GameObject target;
    private float daggerEffectValue;
    private float daggerEffectDuration;
    private bool daggerEffectIsStackable;
    private bool daggerEffectIsRemovable;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, daggerSpeed * Time.deltaTime);

            // rotate the dagger
            Vector3 moveDirection = target.transform.position - transform.position;
            gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == target)
        {
            print("Dagger hit target");
            // apply damage
            target.GetComponent<EnemyBrain>().TakeDamage(daggerDamage);

            // apply effect
            target.GetComponent<EffectSystem>().TakeStatusEffect("kjsbfowfnöwpenäfwapojf0m", "healthMod", daggerEffectValue, daggerEffectDuration, null, daggerEffectIsStackable);

            Destroy(gameObject);
        }
    }

    // SETTERS
    public void SetDaggerSpeed(float speed)
    {
        this.daggerSpeed = speed;
    }

    public void SetDaggerDamage(float damage)
    {
        this.daggerDamage = damage;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void SetDaggerEffectValue(float value)
    {
        this.daggerEffectValue = value;
    }

    public void SetDaggerEffectDuration(float duration)
    {
        this.daggerEffectDuration = duration;
    }

    public void SetDaggerEffectIsStackable(bool stackable)
    {
        this.daggerEffectIsStackable = stackable;
    }
}
