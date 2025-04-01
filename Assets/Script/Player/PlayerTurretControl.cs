using System;
using UnityEngine;

public class PlayerTurretControl : MonoBehaviour
{
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

    private float currentYRotation = 0;
    private float currentXAngle = 0;

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

            this.currentYRotation += mouseX;

            turret.rotation = Quaternion.Euler(0f, currentYRotation, 0f);

            this.currentXAngle += mouseY;
            this.currentXAngle = Mathf.Clamp(this.currentXAngle, depressionAngle, elevationAngle);
            canon.localRotation = Quaternion.Euler(0f, currentXAngle, 0f);


            //    Vector3 moveCrosshair = new Vector3(0f, mouseY, 0f);
            //    crosshair.position += moveCrosshair;
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
