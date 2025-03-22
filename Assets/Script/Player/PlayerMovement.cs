using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 53f;
    [SerializeField] private float rotationSpeed = 25f;
    [SerializeField] private float gravityValue = -9f;
    [SerializeField] private CharacterController characterController;

    private bool playerGrounded;
    private float groundedTimer = 0f;

    // use to store the incremental velocity of gravity to be applied to player when not grounded
    private Vector3 playerVelocity;

    // use to store player input
    private Vector2 playerInput;

    private void Update()
    {
        MovePlayer();
    }

    private void OnMove(InputValue value)
    {
        // Store value recieved from input either keyboard or controller
        playerInput = value.Get<Vector2>();
    }

    private void MovePlayer()
    {
        // Store character controller isGrounded for ease of access
        playerGrounded = characterController.isGrounded;

        //if player is grounded, set our grounded timer to X value.
        //This timer helps prevent the inconsistent nature of isGrounded.
        //This states the player is "grounded" when timer is greater than 0
        if (playerGrounded)
        {
            groundedTimer = 0.2f;
        }

        //If our timer is greater than zero, decrease timer by time.deltaTime
        //This timer will tell us if the player is still "grounded" without the
        //use of the isGrounded var.
        if (groundedTimer > 0)
        {
            groundedTimer -= Time.deltaTime;
        }        
        
        //reset player's vertical velocity to zero when grounded.
        if (playerGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //Increase gravity value over time
        playerVelocity.y += gravityValue * Time.deltaTime;

        //calculate players movement and store in a local vec3
        Vector3 storeMovement = transform.forward * playerInput.y * moveSpeed * moveSpeed * Time.deltaTime;

        //inject player x and z movement back into our player's velocity vec3
        playerVelocity.x = storeMovement.x;
        playerVelocity.z = storeMovement.z;

        //Move player using character controller .move API 
        characterController.Move(playerVelocity * Time.deltaTime);

        //Rotate player using .Rotate API (using rotate around given axis via X amount of degrees)
        transform.Rotate(transform.up, rotationSpeed * playerInput.x * Time.deltaTime);

    }
}
