using System;
using UnityEngine;

public class PlayerTurretControl : MonoBehaviour
{
    private const float MAX_POSITIVE_Y = 486;
    private const float MAX_NEGATIVE_Y = 343;
    private const float RANGE = MAX_POSITIVE_Y - MAX_NEGATIVE_Y;
    [Tooltip("Crosshair speed in pixel by degrees")]
    private const float CROSSHAIR_SPEED = RANGE / 23f;

    [Header("Transform object")]
    [Tooltip("Turret transform")]
    [SerializeField] private Transform turret;
    [Tooltip("Gun transform")]
    [SerializeField] private Transform canon;
    [Tooltip("Crosshair transform")]
    [SerializeField] private Transform crosshair;
    [Space(10)]

    [Tooltip("Player Camera")]
    [SerializeField] private Camera playerCam;
    [Space(10)]

    [Header("Speed Rotation")]
    [Tooltip("Speed traverse of the turret in degrees")]
    [SerializeField] private float speedRotation = 25f;
    [Tooltip("Speed traverse of the gun in degrees")]
    [SerializeField] private float speedInclinaison = 50f;
    [Space(10)]

    [Header("Mouse sensitivity")]
    [Tooltip("X Sensitivity")]
    [SerializeField] private float sensX = 5f;
    [Tooltip("Y Sensitivity")]
    [SerializeField] private float sensY = 3f;
    [Space(10)]

    [Header("Angles")]
    [Tooltip("The minimum angle the gun can get")]
    [SerializeField] private float depressionAngle = -8f;
    [Tooltip("The maximum angle the gun can get")]
    [SerializeField] private float elevationAngle = 15f;
    [Space(10)]

    private float rotationTurret;
    private float inclinaisonGun;

    private float currentXRotation = 0;
    private float currentYAngle = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!MenuPause.isGamePaused)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensX;
            float mouseY = Input.GetAxis("Mouse Y") * sensY;

            this.currentXRotation += mouseX;

            turret.rotation = Quaternion.Euler(0f, currentXRotation, 0f);

            this.currentYAngle += mouseY;
            this.currentYAngle = Mathf.Clamp(this.currentYAngle, depressionAngle, elevationAngle);
            canon.localRotation = Quaternion.Euler(0f, currentYAngle, 0f);

            Debug.Log(crosshair.position);

            if(crosshair.position.y >= MAX_NEGATIVE_Y && crosshair.position.y <= MAX_POSITIVE_Y)
            {
                Vector3 moveCrosshair = new Vector3(0f, mouseY, 0f);
                crosshair.position += moveCrosshair * CROSSHAIR_SPEED;
            }

            if(crosshair.position.y > MAX_POSITIVE_Y)
            {
                crosshair.position = new Vector3(crosshair.position.x, MAX_POSITIVE_Y, 0f);
            }

            if(crosshair.position.y < MAX_NEGATIVE_Y)
            { 
                crosshair.position = new Vector3(crosshair.position.x, MAX_NEGATIVE_Y, 0f);

            }
        }
    }

    private void RotateGun()
    {
        canon.Rotate(inclinaisonGun, 0f, 0f, Space.World);
    }

    private void RotateTurret()
    {
        turret.Rotate(0f, rotationTurret, 0f, Space.World);
    }

    private void DetectMouseInput()
    {
        rotationTurret = Input.GetAxis("Mouse X");
        inclinaisonGun = -Input.GetAxis("Mouse Y");
    }
}
