using UnityEngine;
using UnityEngine.InputSystem;

public class ShoulderFollowCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 shoulderOffset = new Vector3(0.6f, 1.8f, -3f);
    public float followSmoothness = 10f;

    public float rotationInfluence = 0.5f;
    public float rotationSpeed = 6f;
    public float recentreSpeed = 2f;
    public float deadZoneRadius = 0.1f;

    private Vector3 velocity;

    void LateUpdate()
    {
        if (player == null) return;

        // Step 1: Position camera with offset
        Vector3 targetPos = player.position 
            + player.right * shoulderOffset.x 
            + player.up * shoulderOffset.y 
            + player.forward * shoulderOffset.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 1f / followSmoothness);

        // Step 2: Determine mouse offset from screen center
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 offset = (mousePos - screenCenter) / screenCenter; // normalized [-1,1]

        // Apply dead zone
        float offsetMagnitude = offset.magnitude;
        if (offsetMagnitude < deadZoneRadius)
            offset = Vector2.zero;

        // Step 3: Calculate rotation direction
        Vector3 lookTarget = player.position + player.forward * 10f; // default look ahead

        if (offset != Vector2.zero)
        {
            Vector3 sideInfluence = player.right * offset.x * rotationInfluence;
            Vector3 upInfluence = player.up * offset.y * rotationInfluence;
            lookTarget += sideInfluence + upInfluence;
        }

        // Step 4: Smoothly rotate toward target
        Vector3 direction = lookTarget - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}