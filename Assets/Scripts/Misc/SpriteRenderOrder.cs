using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderOrder : MonoBehaviour
{
    void Update()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int) (transform.parent.position.y * -100);
    }
}
