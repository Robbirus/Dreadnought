using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private const string ANIMATOR_IS_MOVING = "isMoving";

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [Space(10)]

    [Header("Input Action Reference")]
    [SerializeField] private InputActionReference moveTurretActionReference;
    [Space(10)]

    [Header("Speed Properties")]
    [SerializeField] private float maxSpeed = 53f;
    [SerializeField] private float maxReverseSpeed = 20f;
    [Space(10)]

    [Header("Turret")]
    [SerializeField] private GameObject turret;
    [SerializeField] private float rotationTurretSpeed = 25f;

    private float currentSpeed;

    public static float forwardBackward;
    public static float leftRight;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        moveTurretActionReference.action.Enable();
        currentSpeed = 0f;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        RotateTurret();
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

    #region Getter / Setter
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
    }
    #endregion
}
