using UnityEngine;
using UnityEngine.InputSystem;

public class TankController : MonoBehaviour
{

    [SerializeField]
    private InputActionReference moveActionReference;
    [SerializeField]
    private InputActionReference boostActionReference;
    //[SerializeField]
    //private InputActionReference shootActionReference;
    [SerializeField]
    private float speed = 20f;
    [SerializeField]
    private float rotationSpeed = 20f; 
    [SerializeField]
    private GameObject shell;

    private float shellSpeed = 500f;
    private Rigidbody rbTank;
    public Transform shellSpawnPoint;

    public static float forwardBackward;
    public static float leftRight;

    private bool forward;
    private bool backward;
    private bool left;
    private bool right;

    private float startTime;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        rbTank = GetComponent<Rigidbody>();
        moveActionReference.action.Enable();
        boostActionReference.action.Enable();
        //shootActionReference.action.Enable();
        speed = 20f;
        rotationSpeed = 20f;
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
        forwardBackward = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        leftRight = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

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

        TankForwardBackward();
        TankLeftRight();

        //if (shootActionReference.action.IsPressed())
        //{
        //    Shoot();
        //}
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(shell, shellSpawnPoint.position, shellSpawnPoint.rotation);
        shell.GetComponent<Rigidbody>().linearVelocity = shellSpawnPoint.forward * shellSpeed;
        Destroy(projectile, 3f);
    }

    private void TankForwardBackward()
    {
        Vector3 moveFB;
        if (boostActionReference.action.IsPressed() == true)
        {
            moveFB = transform.forward * forwardBackward * 5 * speed * Time.deltaTime;
        } 
        else
        {
            moveFB = transform.forward * forwardBackward *  speed * Time.deltaTime;
        }
        rbTank.MovePosition(rbTank.position + moveFB);

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
}
