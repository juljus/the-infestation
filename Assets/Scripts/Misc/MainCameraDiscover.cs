using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraDiscover : MonoBehaviour
{
    // when colliding with a discoverable tile, destroy the tile
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("collided with " + collision.gameObject.tag);
        if (collision.gameObject.tag == "DiscoverableTile")
        {
            Destroy(collision.gameObject);
        }
    }
}
