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
        PlayerMovement();
    }

    void PlayerMovement()
    {
        Vector2 input = this.input.Player.Move.ReadValue<Vector2>();
        
        Vector3 moveDirection = playerCamera.transform.forward * input.y + playerCamera.transform.right * input.x;
        moveDirection.y = 0f;
        moveDirection.Normalize();
        
        Vector3 velocity = moveDirection * speed;
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
}

