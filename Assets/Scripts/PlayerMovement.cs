using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 5f; 
    public float strafeSpeed = 4f;

    private CharacterController controller;

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
        Vector3 forwardMovement = transform.forward * forwardSpeed * Time.deltaTime;

        float horizontalInput = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            horizontalInput = -1f;
        }
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            horizontalInput = 1f;
        }

        Vector3 strafeMovement = transform.right * horizontalInput * strafeSpeed * Time.deltaTime;
        Vector3 totalMovement = forwardMovement + strafeMovement;

        controller.Move(totalMovement);
    }
}