using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraDiscover : MonoBehaviour
{
    // when colliding with a discoverable tile, destroy the tile
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DiscoverableTile")
        {
            Destroy(collision.gameObject);
        }
    }
}
