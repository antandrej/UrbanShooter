using UnityEngine;
using UnityEngine.InputSystem;

public class CrosshairWorldTarget : MonoBehaviour
{
    public float defaultDistance = 10f;
    public LayerMask targetLayers;
    public Transform playerFallback;
    public float smoothing = 10f;

    void Update()
    {
        if (Camera.main == null || playerFallback == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Vector3 targetPoint = ray.GetPoint(defaultDistance);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, targetLayers))
            targetPoint = hit.point;

        Vector3 fallbackTarget = playerFallback.position + playerFallback.forward * defaultDistance;
        Vector3 blendedTarget = Vector3.Lerp(fallbackTarget, targetPoint, 0.8f); // fixed bias or removed if handled elsewhere

        transform.position = Vector3.Lerp(transform.position, blendedTarget, Time.deltaTime * smoothing);
    }
}
