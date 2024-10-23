using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageCatcher : MonoBehaviour
{
    public void TakeDamage(float damage)
    {
        try { transform.GetComponent<Boss1Script>().TakeDamage(damage); }
        catch {}
        try { transform.GetComponent<Boss2Script>().TakeDamage(damage); }
        catch {}
        try { transform.GetComponent<Boss3Script>().TakeDamage(damage); }
        catch {}
    }

    public void Heal(float heal)
    {
        try { transform.GetComponent<Boss1Script>().Heal(heal); }
        catch {}
        try { transform.GetComponent<Boss2Script>().Heal(heal); }
        catch {}
        try { transform.GetComponent<Boss3Script>().Heal(heal); }
        catch {}
    }
}
