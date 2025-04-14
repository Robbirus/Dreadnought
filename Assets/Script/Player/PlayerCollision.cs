using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
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
}
