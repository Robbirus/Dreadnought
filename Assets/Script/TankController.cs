using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TankController : MonoBehaviour
{
    private const string ANIMATOR_IS_MOVING = "isMoving";

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private InputActionReference moveActionReference;
    [SerializeField]
    private InputActionReference moveTurretActionReference;

    [SerializeField]
    private float maxHealth = 100f;
    [SerializeField]
    private float chipSpeed = 2f;
    [SerializeField]
    private Image frontHealthBar;
    [SerializeField]
    private Image backHealthBar;

    [SerializeField]
    private float maxSpeed = 50f;
    [SerializeField]
    private float rotationSpeed = 20f;
    [SerializeField]
    private float rotationTurretSpeed = 25f;

    [SerializeField]
    private GameObject turret; 

    private Rigidbody rbTank;

    private float decelerationRate = 1f;
    private Vector3 direction;
    public static float forwardBackward;
    public static float leftRight;

    private bool forward;
    private bool backward;
    private bool left;
    private bool right;

    private float health;
    private float lerpTimer;


    private float currentSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        rbTank = GetComponent<Rigidbody>();
        moveActionReference.action.Enable();
        moveTurretActionReference.action.Enable();
        direction = transform.forward;

        health = maxHealth;
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
        // UI
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        if (Input.GetKey(KeyCode.X))
        {
            TakeDamage(Random.Range(5, 10));
        }
        if (Input.GetKey(KeyCode.V))
        {
            RestoreHealth(Random.Range(5, 10));
        }

        // Movement
        direction = transform.forward;

        forwardBackward = Input.GetAxis("Vertical");
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

        if (forwardBackward == 0f)
        {
            Decelerate();

        }
        else
        {
            Accelerate();
        }
        RotateTurret();
        TankLeftRight();

    }

    public void UpdateHealthUI()
    {
        Debug.Log(health);
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = health / maxHealth;

        if(fillBack > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.fixedDeltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }

        if(fillFront < healthFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = healthFraction;
            lerpTimer += Time.fixedDeltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }


    private float CalculateAccelerate(float currentSpeed)
    {
        // Si la vitesse maximale est atteinte, l'acceleration est nulle
        if (currentSpeed >= maxSpeed)
        {
            return 0;
        }

        // Calcul de l'acceleration 
        return (maxSpeed - currentSpeed) / 2f;
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

            animator.SetBool(ANIMATOR_IS_MOVING, true);
        } 
        else
        {
            Debug.LogWarning("Vector is null, no force applied");
        }
    }

    private void TankLeftRight()
    {
        Quaternion rotateLR = Quaternion.Euler(0f, leftRight, 0f);
        rbTank.MoveRotation(rbTank.rotation * rotateLR);
    }

    private void SwitchBoolsOff()
    {
        forward = false;
        backward = false;
        left = false;
        right = false;

    }

    public float GetCurrentSpeed()
    {
        return this.currentSpeed;
    }
}
