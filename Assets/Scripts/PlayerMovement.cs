using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float baseForwardSpeed = 5f;
    public float slowMultiplier = 0.5f;
    public float boostMultiplier = 1.5f;
    public float strafeSpeed = 4f;

    public float slideDuration = 0.6f;
    public float slideHeight = 1.2f;
    public float slideDistance = 2f;

    public float gravity = -9.81f;

    private CharacterController controller;
    private float currentForwardSpeed;
    private Vector3 verticalVelocity = Vector3.zero;

    private bool isSliding = false;
    private float slideTimer = 0f;
    private Vector3 slideStart;
    private Vector3 slideEnd;

    private PlayerShooting isAlive;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        isAlive = GetComponent<PlayerShooting>();
        if (controller == null)
        {
            Debug.LogError("CharacterController not found.");
        }
    }

    void Update()
    {
        if (isAlive.isAlive)
        {
            if (isSliding)
            {
                slideTimer += Time.deltaTime;
                float t = slideTimer / slideDuration;

                if (t >= 1f)
                {
                    controller.Move(slideEnd - transform.position + Vector3.down * 0.1f);
                    isSliding = false;
                }
                else
                {
                    Vector3 flatMove = Vector3.Lerp(slideStart, slideEnd, t);
                    float height = Mathf.Sin(t * Mathf.PI) * slideHeight;
                    flatMove.y += height;

                    Vector3 delta = flatMove - transform.position;
                    controller.Move(delta);
                }

                return;
            }

            bool slowingDown = Keyboard.current.sKey.isPressed;
            bool speedingUp = Keyboard.current.wKey.isPressed;

            float speedFactor = 1f;
            if (slowingDown) speedFactor = slowMultiplier;
            else if (speedingUp) speedFactor = boostMultiplier;

            currentForwardSpeed = baseForwardSpeed * speedFactor;

            float horizontalInput = 0f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                horizontalInput = -1f;
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                horizontalInput = 1f;

            Vector3 move = transform.forward * currentForwardSpeed + transform.right * horizontalInput * strafeSpeed;

            if (!controller.isGrounded)
                verticalVelocity.y += gravity * Time.deltaTime;
            else
                verticalVelocity.y = -1f;

            move += verticalVelocity;

            controller.Move(move * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && !isSliding && isAlive.isAlive)
        {
            slideStart = transform.position;
            float adjustedHeight = 2f;
            float halfPlayerHeight = controller.height / 2f;

            slideEnd = new Vector3(
                transform.position.x,
                adjustedHeight - halfPlayerHeight,
                transform.position.z + 1.5f
            );

            slideTimer = 0f;
            isSliding = true;
        }
    }
}