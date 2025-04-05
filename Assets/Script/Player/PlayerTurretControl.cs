using System;
using UnityEngine;

public class PlayerTurretControl : MonoBehaviour
{
    [Header("Transform object")]
    [Tooltip("Turret transform")]
    [SerializeField] private Transform turret;
    [Tooltip("Gun transform")]
    [SerializeField] private Transform canon;
    [Tooltip("Crosshair UI transform")]
    [SerializeField] private Transform crosshairUI;
    [Tooltip("Shell Spawn Point Transform")]
    [SerializeField] private Transform firePoint;
    [Space(10)]

    [Tooltip("Player Camera")]
    [SerializeField] private Camera playerCam;
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

            UpdateCrosshairUI();

        }
    }

    private void UpdateCrosshairUI()
    {
        Ray ray = new Ray(firePoint.position, firePoint.forward);
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            //Debug.Log("Something : " + hit.collider.gameObject.name);
            targetPoint = hit.point;
            Debug.DrawRay(firePoint.position, firePoint.forward * 1000f, Color.green);
        }
        else
        {
            targetPoint = firePoint.position + firePoint.forward * 1000f;
            Debug.DrawRay(firePoint.position, firePoint.forward * 1000f, Color.red);
        }

        if (CheckVisible(hit.collider))
        {
            Vector3 screenPos = playerCam.WorldToScreenPoint(targetPoint);
            crosshairUI.position = screenPos;
        }
    }

    private bool CheckVisible(Collider collider)
    {
        bool visible = true;

        if (collider == null)
        {
            visible = false;
        }
        else
        {
            switch (collider.gameObject.name)
            {
                case "Shell(Clone)":
                    visible = false;
                    break;
            }
        }

        return visible;
    }
}
