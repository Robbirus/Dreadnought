using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using static UnityEngine.UI.Image;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 53f;
    [SerializeField] private float speedRate = 200f;
    [SerializeField] private float rotSpeedRate = 200f;
    [SerializeField] private float rotationSpeedDegrees = 25f;
    [Space(10)]

    [Header("Ground")]
    [SerializeField] private float groundFiction = 5f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private LayerMask ground;
    [Space(10)]

    [Header("Player")]
    [SerializeField] private float playerHeight;
    [Space(10)]

    private Rigidbody rb;
    private Vector3 groundNormal = Vector3.up;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
        AdjustToGround();

        rb.AddForce(-transform.up * gravity, ForceMode.Acceleration);
    }

    private void HandleMovement()
    {
        // Avant/arriere
        float moveInput = Input.GetAxis("Vertical"); 
        Vector3 force = transform.forward * moveInput * speedRate * Time.fixedDeltaTime;

        // Applique la force seulement si le tank touche le sol
        if (IsGrounded())
        {
            rb.AddForce(force * 10f, ForceMode.Acceleration);
        }

        if (moveInput == 0)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 5f);
        }
    }

    private void HandleRotation()
    {
        // Gauche / Droite
        float turnInput = Input.GetAxis("Horizontal");

        if (IsGrounded())
        {
            // Applique une force de rotation en fonction de la friction du sol
            rb.AddTorque(Vector3.up * turnInput * rotSpeedRate * Time.fixedDeltaTime, ForceMode.Acceleration);

            // Reduction du glissement
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, transform.forward * rb.linearVelocity.magnitude, Time.fixedDeltaTime * groundFiction);
        }
    }

    private void AdjustToGround()
    {
        RaycastHit hit;
        Vector3 tankPosition = transform.position;

        // Raycast vers le sol pour obtenir la normale (la direction perpendiculaire au sol)
        if (Physics.Raycast(tankPosition, -transform.up, out hit, 3f))
        {
            // Calcule la nouvelle rotation alignee avec la pente
            Quaternion newRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            // Lisse la transition pour eviter des changements brutaux
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5f);
        }
    }

    private bool IsGrounded()
    {
        float distance = 1f; // Distance correcte
        float radius = 0.5f; // Rayon du tank
        float offsetY = 1f;
        bool grounded = true;

        Vector3 front = transform.position + Vector3.up * offsetY + transform.forward * 4f; // Avant du tank
        Vector3 back = transform.position + Vector3.up * offsetY - transform.forward * 9f;  // Arriere du tank
        Vector3 right = transform.position + Vector3.up * offsetY + transform.right * 3f;   // Cote droit du tank
        Vector3 left = transform.position + Vector3.up * offsetY - transform.right * 3f;    // Cote gauche du tank
        Vector3 center = transform.position + Vector3.up * offsetY;                         // Centre du tank

        Vector3[] raycastsOrigins = new Vector3[]
        {
            front,
            back,
            right,
            left,
            center
        };

        foreach (Vector3 origin in raycastsOrigins)
        {
            grounded = Physics.SphereCast(origin, radius, -Vector3.up, out RaycastHit hit, distance);

            Debug.DrawRay(front, -Vector3.up * distance, grounded ? Color.green : Color.red);
            Debug.DrawRay(back, -Vector3.up * distance, grounded ? Color.green : Color.red);
            Debug.DrawRay(right, -Vector3.up * distance, grounded ? Color.green : Color.red);
            Debug.DrawRay(left, -Vector3.up * distance, grounded ? Color.green : Color.red);
            Debug.DrawRay(center, -Vector3.up * distance, grounded ? Color.green : Color.red);

            /*
            if (grounded)
                Debug.Log($"[OK] Sol detecte avec SphereCast : {hit.collider.name}");
            else
                Debug.Log("[ERREUR] Aucun sol detecte !");
            */
            return grounded;
        }

        return false;
    }
}
