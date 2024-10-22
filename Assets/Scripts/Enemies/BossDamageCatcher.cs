using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageCatcher : MonoBehaviour
{
    public void TakeDamage(float damage)
    {
        transform.GetComponent<Boss1Script>().TakeDamage(damage);
        // transform.GetComponent<Boss2Script>().TakeDamage(damage);
        // transform.GetComponent<Boss3Script>().TakeDamage(damage);
    }

    public void Heal(float heal)
    {
        transform.GetComponent<Boss1Script>().Heal(heal);
        // transform.GetComponent<Boss2Script>().Heal(heal);
        // transform.GetComponent<Boss3Script>().Heal(heal);
    }
}
