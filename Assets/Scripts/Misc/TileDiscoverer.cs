using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDiscoverer : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("GameManager").GetComponent<PlayerManager>().GetPlayer;
    }

    private void Update()
    {
        // set position to player position
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
    }

    // when colliding with a discoverable tile, destroy the tile
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DiscoverableTile")
        {
            Destroy(collision.gameObject);
        }
    }
}
