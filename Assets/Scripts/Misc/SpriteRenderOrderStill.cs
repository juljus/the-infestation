using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderOrderStill : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int) (transform.parent.position.y * -100);
    }
}
