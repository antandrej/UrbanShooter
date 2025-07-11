using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class ShoulderFollowCamera : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Position Settings")]
    public Vector3 defaultShoulderOffset = new Vector3(0.6f, 1.8f, -3f);
    public Vector3 aimShoulderOffset = new Vector3(0.35f, 1.6f, -2f);
    public float followSmoothness = 10f;

    [Header("FOV Settings")]
    public float defaultFOV = 60f;
    public float aimFOV = 45f;
    public float fovSmoothness = 10f;

    [Header("Rotation Settings")]
    public float rotationInfluence = 0.5f;
    public float rotationSpeed = 6f;
    public float deadZoneRadius = 0.1f;

    [Header("Slide Tilt")]
    public PlayerMovement playerMovement;
    public float slideRollAmount = 8f;
    public float slideRollSpeed = 6f;

    private float currentRollZ = 0f;


    private Camera cam;
    private Vector3 velocity;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.fieldOfView = defaultFOV;
    }

    void LateUpdate()
    {
        if (player == null) return;

        bool isAiming = Mouse.current.rightButton.isPressed;

        Vector3 targetOffset = isAiming ? aimShoulderOffset : defaultShoulderOffset;
        Vector3 desiredPos = player.position
            + player.right * targetOffset.x
            + player.up * targetOffset.y
            + player.forward * targetOffset.z;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, 1f / followSmoothness);

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 offset = (mousePos - screenCenter) / screenCenter;

        float offsetMag = offset.magnitude;
        float aimBias = Mathf.InverseLerp(deadZoneRadius, 1f, offsetMag);

        Vector3 lookTarget = player.position + player.forward * 10f;
        lookTarget += player.right * offset.x * rotationInfluence * aimBias;
        lookTarget += player.up * offset.y * rotationInfluence * aimBias;

        Quaternion targetRot = Quaternion.LookRotation(lookTarget - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);

        // Apply slide-based Z-axis roll
        float targetRoll = (playerMovement != null && playerMovement.IsSliding) ? slideRollAmount : 0f;
        currentRollZ = Mathf.Lerp(currentRollZ, targetRoll, Time.deltaTime * slideRollSpeed);

        // Combine roll with rotation
        Quaternion rollRotation = Quaternion.Euler(0f, 0f, currentRollZ);
        transform.rotation = targetRot * rollRotation;


        float targetFOV = isAiming ? aimFOV : defaultFOV;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * fovSmoothness);
    }
}