
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private int velocity = 50;
    [SerializeField] private float rotationSpeed = 25f;
    [SerializeField] private int power = 950;
    [SerializeField] private float accelerationTime = 10f;
    [Space(10)]

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private float groundDrag;
    [Space(10)]

    private bool isGrounded;
    private Rigidbody rb;
    private float mass;
    private float acceleration;
    private float powerToWeightRatio;
    private Vector3 moveDirection;
    private float force = 0f;

    private float horizontalInput;
    private float verticalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        mass = rb.mass;
        powerToWeightRatio = power / mass;
        acceleration = (2 * power) / (mass * accelerationTime);
        force = acceleration * mass;
    }

    
    private void Update()
    {
        // Debug.Log(isGrounded);
        if (isGrounded)
        {
            DetectInput();
            ControlSpeed();
            rb.linearDamping = groundDrag; 
        }
        else
        {
            rb.angularDamping = 0;
        }
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            MovePlayer();
            TurnPlayer();
        }
    }

    private void DetectInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    /// <summary>
    /// Move forward or backward the player
    /// </summary>
    private void MovePlayer()
    {
        moveDirection = transform.forward * verticalInput;
        rb.AddForce(moveDirection.normalized * force * 50f, ForceMode.Force);
    }

    /// <summary>
    /// Rotate the player on himself
    /// </summary>
    private void TurnPlayer()
    {
        float turnInput = horizontalInput * rotationSpeed * Time.deltaTime;
        Quaternion rotate = Quaternion.Euler(0f, turnInput, 0f);
        rb.MoveRotation(rb.rotation * rotate);
    }

    /// <summary>
    /// Limit the gain of acceleration
    /// </summary>
    private void ControlSpeed()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if(flatVel.magnitude > velocity)
        {
            Vector3 limitedVel = flatVel.normalized * velocity;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
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
    #endregion

}
