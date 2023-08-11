using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float zoomOutSize = 8f;
    [SerializeField] private float zoomInSize = 5f;
    [SerializeField] private float zoomSpeed = 5f;

    private Camera cam;
    private Vector3 originalPosition;
    private bool isZooming = false;

    private void Start()
    {
        cam = GetComponent<Camera>();
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (isZooming)
        {
            ZoomCamera();
        }
    }

    public void ZoomOut()
    {
        isZooming = true;
    }

    public void ZoomIn()
    {
        isZooming = false;
        cam.orthographicSize = zoomInSize;
        transform.position = originalPosition;
    }

    private void ZoomCamera()
    {
        float targetSize = isZooming ? zoomOutSize : zoomInSize;
        Vector3 targetPosition = isZooming ? new Vector3(originalPosition.x, originalPosition.y, -10f) : originalPosition;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * zoomSpeed);
    }
}
