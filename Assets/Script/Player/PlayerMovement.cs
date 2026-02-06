using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Base player's rotation speed in degrees")]
    [SerializeField] private float rotationSpeed = 25f;
    [Tooltip("Base player's engine power in hp")]
    [SerializeField] private int power = 950;
    [Tooltip("Base player's acceleration time")]
    [SerializeField] private float accelerationTime = 100f;
    [Space(10)]

    [Header("Ground Check")]
    [Tooltip("Height of the player")]
    [SerializeField] private float playerHeight;
    [Tooltip("The ground resistance, the higher this value is, the higher is the grip")]
    [SerializeField] private float groundDrag;
    [Space(10)]

    [Header("Speed")]
    [SerializeField] private float maxSpeed = 60f;
    [SerializeField] private float deceleration = 1f;
    [Space(10)]

    [Header("Physics")]
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private InputActionReference movementActionReference;

    private bool isGrounded;
    private float acceleration;

    #region Old Input System variables
    private float horizontalInput;
    private float verticalInput;
    #endregion

    #region Speed value
    private float currentSpeed = 0f;
    #endregion

    private void Awake()
    {
        movementActionReference.action.Enable();
    }

    private void Start()
    {
        ComputeAcceleration();
    }

    private void ComputeAcceleration()
    {
        acceleration = (2 * power) / (playerRigidbody.mass * accelerationTime);
    }

    private void Update()
    {
        DetectInput();
    }

    private void FixedUpdate()
    {
        if (!isGrounded) return;

        UpdateSpeed();
        MovingPlayer();
        TurnPlayer();
        
    }

    private void UpdateSpeed()
    {
        if(Mathf.Abs(verticalInput) > 0.01f)
        {
            currentSpeed += verticalInput * acceleration;
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration);
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
    }

    private void MovingPlayer()
    {
        Vector3 move = transform.forward * currentSpeed * Time.fixedDeltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + move);
    }

    private void DetectInput()
    {
        Vector2 movement = movementActionReference.action.ReadValue<Vector2>();

        horizontalInput = movement.x;
        verticalInput = movement.y;

    }

    /// <summary>
    /// Rotate the player on himself
    /// </summary>
    private void TurnPlayer()
    {
        float turnInput = horizontalInput * rotationSpeed * Time.deltaTime;
        Quaternion rotate = Quaternion.Euler(0f, turnInput, 0f);
        playerRigidbody.MoveRotation(playerRigidbody.rotation * rotate);
    }

    #region Getter / Setter
    public void SetGrounded(bool isGrounded)
    {
        this.isGrounded = isGrounded;
    }

    public bool IsGrounded()
    {
        return this.isGrounded;
    }

    public float GetCurrentSpeed()
    {
        return this.currentSpeed;
    }

    public float GetMaxSpeed()
    {
        return this.maxSpeed;
    }

    public void SetMaxSpeed(float maxSpeed)
    {
        this.maxSpeed = maxSpeed;
    }

    public float GetReverseSpeed()
    {
        return this.maxSpeed;
    }

    public void SetRotationSpeed(float rotationSpeed)
    {
        this.rotationSpeed = rotationSpeed;
    }
    #endregion
}
