using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public float minZoom = 2f;
    public float maxZoom = 10f;
    public float zoomSpeed = 0.01f;  // Lowered for better control
    public float moveSpeed = 0.01f;  // Lowered to match screen resolution

    private Camera cam;
    private Vector2 lastTouchPosition;
    private bool isDragging = false;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount == 1) // Single touch for panning
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = (touch.position - lastTouchPosition) * moveSpeed;
                transform.position -= new Vector3(delta.x, delta.y, 0);
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
        else if (Input.touchCount == 2) // Pinch to zoom
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            float prevDistance = (touch1.position - touch1.deltaPosition - (touch2.position - touch2.deltaPosition)).magnitude;
            float currentDistance = (touch1.position - touch2.position).magnitude;

            float zoomChange = (currentDistance - prevDistance) * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - zoomChange, minZoom, maxZoom);
        }
    }
}
