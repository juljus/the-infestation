using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    private Transform player;
    
    void Start()
    {
        player = transform.GetComponent<PlayerManager>().GetPlayerTransform;
    }

    void Update()
    {
        mainCamera.transform.position = new Vector3(player.position.x, player.position.y, -10);
    }
}
