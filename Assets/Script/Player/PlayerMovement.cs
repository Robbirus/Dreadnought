using UnityEngine;

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

    [Header("GameObject UI Instance")]
    [SerializeField] private GameObject needle;

    #region Needle attributes
    private float startPosition = 220f;
    private float endPosition = -41f;
    private float desiredPosition;
    #endregion

    #region Physics Attributs
    private bool isGrounded;
    private Rigidbody rb;
    private float mass;
    public float acceleration;
    private Vector3 moveDirection;
    private float force = 0f;
    #endregion

    #region Old Input System variables
    private float horizontalInput;
    private float verticalInput;
    #endregion

    #region Speed value
    public float maxSpeed = 50f;
    public float reverseSpeed = 23f;
    public float currentSpeed = 0f;
    public float deceleration = 1f;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        mass = rb.mass;
        acceleration = (2 * power) / (mass * accelerationTime);
        force = acceleration * mass;
    }

    
    private void Update()
    {
        acceleration = (2 * power) / (mass * accelerationTime);

        if (isGrounded)
        {
            DetectInput();

            if (verticalInput != 0)
            {
                moveDirection = transform.forward * verticalInput;
                if (moveDirection.magnitude > 0)
                {
                    currentSpeed += acceleration;
                }
            }   
            else
            {
                if(currentSpeed != 0)
                {
                    currentSpeed -= deceleration;
                }
            }

            LimitSpeed();
        }

        UpdateNeedle();
    }

    private void FixedUpdate()
    {
        if (isGrounded) 
        {
            MovingPlayer();
            TurnPlayer();
        }
    }

    private void MovingPlayer()
    {
        Vector3 move = moveDirection * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    private void LimitSpeed()
    {
        if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }
        else if(currentSpeed < -maxSpeed)
        {
            currentSpeed = -maxSpeed;
        }
        else if(verticalInput == 0 && currentSpeed < 0)
        {
            currentSpeed = 0;
        }
    }

    private void DetectInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
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

    private void UpdateNeedle()
    {
        desiredPosition = startPosition - endPosition;
        float temp = currentSpeed / 180;
        needle.transform.eulerAngles = new Vector3(0, 0, (startPosition - temp * desiredPosition));
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
    public void SetReverseSpeed(float reverseSpeed)
    {
        this.reverseSpeed = reverseSpeed;
    }
    #endregion

}
