using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        ProcessCollision(collider.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ProcessCollision(collision.gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameObject.GetComponent<PlayerMovement>().SetGrounded(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameObject.GetComponent<PlayerMovement>().SetGrounded(false);
        }
    }

    private void ProcessCollision(GameObject collider)
    {
        // If the player is in contacts with the ground
        
    }
}
