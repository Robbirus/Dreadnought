using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TankController : MonoBehaviour
{
    private const string ANIMATOR_IS_MOVING = "isMoving";

    [Header("Animator")]
    [SerializeField]
    private Animator animator;

    [Header("Input Action Reference")]
    [SerializeField]
    private InputActionReference moveActionReference;
    [SerializeField]
    private InputActionReference moveTurretActionReference;

    [Header("Speed Properties")]
    [SerializeField]
    private float maxSpeed = 53f;
    [SerializeField]
    private float maxReverseSpeed = 20f;
    [SerializeField]
    private float accelerationRate = 500f;
    [SerializeField]
    private float decelerationRate = 300f;
    [SerializeField]
    private float rotationSpeed = 20f;
    [SerializeField]
    private float rotationTurretSpeed = 25f;

    [Header("GameObject Instance")]
    [SerializeField]
    private GameObject turret; 

    private Rigidbody rbTank;
    private float currentSpeed;

    private Vector3 direction;
    public static float forwardBackward;
    public static float leftRight;

    private bool forward;
    private bool backward;
    private bool left;
    private bool right;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        rbTank = GetComponent<Rigidbody>();
        moveActionReference.action.Enable();
        moveTurretActionReference.action.Enable();
        direction = transform.forward;
        currentSpeed = 0f;

    }

    public void Forward()
    {
        forward = true;
    }

    public void Backward()
    {
        backward = true;
    }

    public void Left()
    {
        left = true;
    }

    public void Right()
    {
        right = true;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Movement
        direction = transform.forward;

        forwardBackward = Input.GetAxis("Vertical") * maxSpeed * Time.fixedDeltaTime;
        leftRight = Input.GetAxis("Horizontal") * rotationSpeed * Time.fixedDeltaTime;

        if (forward)
        {
            forwardBackward = -1f;
        }
        if (backward)
        {
            forwardBackward = 1f;
        }
        if (left)
        {
            leftRight = -1f;
        }
        if (right)
        {
            leftRight = 1f;
        }

        // Utilisation de l'ancien systeme de deplacement
        /*
        if (forwardBackward == 0f)
        {
            Decelerate();

        }
        else
        {
            Accelerate();
        }
        */

        TankFowardBack();
        TankLeftRight();
        RotateTurret();
    }

    private void TankFowardBack()
    {
        Vector3 moveFB;
        if (forwardBackward < 0)
        {
            moveFB = transform.forward * forwardBackward * maxReverseSpeed * Time.fixedDeltaTime;
            currentSpeed = maxReverseSpeed;
        }
        else
        {
            moveFB = transform.forward * forwardBackward * maxSpeed * Time.fixedDeltaTime;
            currentSpeed = maxSpeed;
        }
        rbTank.MovePosition(rbTank.position + moveFB);
    }

    private void RotateTurret()
    {
        Vector3 rotationAxis = Vector3.zero;
        float step = rotationTurretSpeed * Time.fixedDeltaTime;
        float value = moveTurretActionReference.action.ReadValue<float>();
        if (value == 0) return;

        if(value < 0)
        {
            rotationAxis = Vector3.down;
        }
        if (value > 0)
        {
            rotationAxis = Vector3.up;
        }
        turret.transform.Rotate(rotationAxis * step, Space.Self);
    }
    private void TankLeftRight()
    {
        Quaternion rotateLR = Quaternion.Euler(0f, leftRight, 0f);
        rbTank.MoveRotation(rbTank.rotation * rotateLR);
    }

    private float CalculateAccelerate(float currentSpeed)
    {
        // Si la vitesse maximale est atteinte, l'acceleration est nulle
        if (currentSpeed >= maxSpeed)
        {
            return 0;
        }

        // Calcul de l'acceleration 
        return (maxSpeed - currentSpeed) / accelerationRate;
    }

    private void Decelerate()
    {
        // Obtention de la vitesse actuel
        this.currentSpeed = rbTank.linearVelocity.magnitude;
        if (this.currentSpeed >= 0)
        {
            // Calcul de la force opposee pour ralentir le tank
            Vector3 decelerationForce = - rbTank.linearVelocity * decelerationRate * rbTank.mass;
            rbTank.AddForce(decelerationForce);

            // Empecher que la vitesse devienne negative
            if(rbTank.linearVelocity.magnitude < 0.1f)
            {
                rbTank.linearVelocity = Vector3.zero;
            }

            animator.SetBool(ANIMATOR_IS_MOVING, decelerationForce.magnitude > 0);
        }
    }

    private void Accelerate()
    {
        // Obtention de la vitesse actuel
        this.currentSpeed = rbTank.linearVelocity.magnitude;

        // calcul de l'acceleration
        float acceleration = CalculateAccelerate(this.currentSpeed);

        if (direction != Vector3.zero) 
        {
            // Application de la force dans la direction specifiee
            Vector3 force = direction * forwardBackward * acceleration * rbTank.mass;
            rbTank.AddForce(force);

            // Limitation de la vitesse
            if (rbTank.linearVelocity.magnitude > maxSpeed)
            {
                rbTank.linearVelocity = rbTank.linearVelocity * maxSpeed;
            }

            animator.SetBool(ANIMATOR_IS_MOVING, force.magnitude > 0);
        } 
        else
        {
            Debug.LogWarning("Vector is null, no force applied");
        }
    }

    private void SwitchBoolsOff()
    {
        forward = false;
        backward = false;
        left = false;
        right = false;
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
        return this.maxReverseSpeed;
    }
    public void SetReverseSpeed(float maxReverseSpeed)
    {
        this.maxReverseSpeed = maxReverseSpeed;
    }
    public float GetCurrentSpeed()
    {
        return this.currentSpeed;
        // A REMETTRE Quand l'acceleration sera regler
        //return this.currentSpeed;
    }
}
