using System;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundLayer;

    private InputSystem_Actions input;
    
    private void Awake()
    {
        input = new InputSystem_Actions();
        input.Enable();
    }

    private void Update()
    {
        Jump();
    }

    void FixedUpdate()
    {
        PlayerMovement(AdvancedMovement());
    }

    void PlayerMovement(float actualSpeed)
    {
        Vector2 input = this.input.Player.Move.ReadValue<Vector2>();
        
        Vector3 moveDirection = playerCamera.transform.forward * input.y + playerCamera.transform.right * input.x;
        moveDirection.y = 0f;
        moveDirection.Normalize();
        
        Vector3 velocity = moveDirection * actualSpeed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }
    
    void Jump()
    {
        RaycastHit hit;
        bool isGrounded = Physics.Raycast(playerCollider.bounds.center, Vector3.down, out hit, 2f, groundLayer);

        if (isGrounded && input.Player.Jump.WasPressedThisFrame())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    float AdvancedMovement()
    {
        Vector2 playerInput = input.Player.Move.ReadValue<Vector2>();
        
        bool isWalkingStraight = playerInput.y > 0.1 && Math.Abs(playerInput.x) < 0.1;
        bool isWalkingBackwards = playerInput.y < -0.1 && Math.Abs(playerInput.x) < 0.1;
        bool isWalkingLeft = Math.Abs(playerInput.y) < 0.1 && playerInput.x > 0.1;
        bool isWalkingRight = Math.Abs(playerInput.y) < 0.1 && playerInput.x < -0.1;
        bool isRunning = isWalkingStraight && input.Player.Sprint.inProgress;

        if (isRunning) return speed * 2;
        if (isWalkingStraight) return speed;
        if (isWalkingBackwards) return speed / 2;
        if (isWalkingLeft || isWalkingRight) return speed - speed * 0.25f;

        return 0;
    }
}

