using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking.PlayerConnection;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float jetpackForce = 3f;
    [SerializeField] private LayerMask groundLayer;

    private InputSystem_Actions _input;
    private float _rayLength;
    private float _jetpackDuration = 1f;
    private float _timer = 0f;
    private bool _canFly;
    
    private void Awake()
    {
        _input = new InputSystem_Actions();
        _input.Enable();
    }

    private void Start()
    {
        _rayLength = playerCollider.height / 2;
    }

    private void Update()
    {
        if (IsGrounded()) _timer = 0f;
        Jump(IsGrounded());
        JetPack();
    }

    void FixedUpdate()
    {
        PlayerMovement(AdvancedMovement(IsGrounded()));
    }

    void PlayerMovement(float actualSpeed)
    {
        Vector2 input = this._input.Player.Move.ReadValue<Vector2>();
        
        Vector3 moveDirection = playerCamera.transform.forward * input.y + playerCamera.transform.right * input.x;
        moveDirection.y = 0f;
        moveDirection.Normalize();
        
        Vector3 velocity = moveDirection * actualSpeed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;
    }
    
    void Jump(bool isGrounded)
    {
        if (isGrounded && _input.Player.Jump.WasPressedThisFrame())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            _canFly = true;
        }
    }

    float AdvancedMovement(bool isGrounded)
    {
        Vector2 playerInput = _input.Player.Move.ReadValue<Vector2>();
        bool isRunning = isGrounded && _input.Player.Sprint.inProgress;
        float actualSpeed;

        if (playerInput.y > 0.1f) actualSpeed = isRunning ? speed * 2 : speed;
        else if (playerInput.y < -0.1) actualSpeed = speed * 0.5f;
        else actualSpeed = speed * 0.75f;

        if (!isGrounded) actualSpeed *= 0.5f;

        return actualSpeed;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(playerCollider.bounds.center, Vector3.down, _rayLength, groundLayer);
    }

    void JetPack()
    {
        if (_canFly && _input.Player.Jump.inProgress && _timer < 1f)
        {
            rb.AddForce(Vector3.up * jetpackForce, ForceMode.Force);
            _timer += Time.deltaTime;
        }

        if (_timer > 1f) _canFly = false;
    }
}

