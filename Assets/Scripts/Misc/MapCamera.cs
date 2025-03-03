using UnityEngine;

public class MapCamera : MonoBehaviour
{
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float zoomSpeed = 0.01f;
    [SerializeField] private float moveSpeed = 0.01f;
    [SerializeField] private Camera cam;

    private Vector2 lastTouchPosition;

    void Update()
    {
        Debug.Log("Touch Count: " + Input.touchCount); // Add this line

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Debug.Log("Touch Phase: " + touch.phase); // Add this line

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = (touch.position - lastTouchPosition) * moveSpeed * cam.orthographicSize;
                transform.position -= new Vector3(delta.x, delta.y, 0);
                lastTouchPosition = touch.position;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            float prevDistance = (touch1.position - touch1.deltaPosition - (touch2.position - touch2.deltaPosition)).magnitude;
            float currentDistance = (touch1.position - touch2.position).magnitude;

            float zoomChange = (currentDistance - prevDistance) * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - zoomChange, minZoom, maxZoom);

            Vector2 pinchMidPoint = (touch1.position + touch2.position) / 2f;
            Vector3 worldPinchMidPoint = cam.ScreenToWorldPoint(pinchMidPoint);
            Vector3 cameraDelta = transform.position - worldPinchMidPoint;

            // Scale the cameraDelta based on the zoom level
            cameraDelta *= Time.deltaTime * cam.orthographicSize * 2; //Adjust the 2 value to increase or decrease speed.

            transform.position += cameraDelta;
        }
    }
}