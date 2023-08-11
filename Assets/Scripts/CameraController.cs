using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float zoomOutSize = 8f;
    [SerializeField] private float zoomInSize = 5f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float followSpeed = 5f;

    private Camera cam;

    private static CameraController instance;
    public static CameraController Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        cam = Camera.main;
    }

    public void ZoomOut()
    {
        cam.DOOrthoSize(zoomOutSize, zoomSpeed);
    }

    public void ZoomIn(Transform target)
    {
        cam.DOOrthoSize(zoomInSize, zoomSpeed);
        FollowTransform(target, 1f);
    }

    public void FollowTransform(Transform target, float duration)
    {
        var targetPosition = new Vector3(target.position.x, target.position.y, -10f);
        cam.transform.DOMove(targetPosition, duration).SetEase(Ease.OutQuad);
    }
}
