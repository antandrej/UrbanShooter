using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float baseForwardSpeed = 5f;
    public float slowMultiplier;
    public float boostMultiplier;
    public float strafeSpeed = 4f;

    private CharacterController controller;

    private float currentForwardSpeed;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController not found.");
        }
    }

    void Update()
{
    bool slowingDown = Keyboard.current.sKey.isPressed;
    bool speedingUp = Keyboard.current.wKey.isPressed;

    float speedFactor = 1f;

    if (slowingDown)
        speedFactor = slowMultiplier;
    else if (speedingUp)
        speedFactor = boostMultiplier;

    currentForwardSpeed = baseForwardSpeed * speedFactor;

    Vector3 forwardMovement = transform.forward * currentForwardSpeed * Time.deltaTime;

    float horizontalInput = 0f;
    if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        horizontalInput = -1f;
    else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        horizontalInput = 1f;

    Vector3 strafeMovement = transform.right * horizontalInput * strafeSpeed * Time.deltaTime;
    Vector3 totalMovement = forwardMovement + strafeMovement;

    controller.Move(totalMovement);
}
}