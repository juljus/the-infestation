using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderOrder : MonoBehaviour
{
    void Update()
    {
        // gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int) (transform.parent.position.y * -100);

        // chage the transform pos of the gameobject
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.parent.position.y);
    }
}
