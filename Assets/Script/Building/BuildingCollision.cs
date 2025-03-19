using UnityEngine;

public class BuildingCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        ProcessCollision(collider.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ProcessCollision(collision.gameObject);
    }

    private void ProcessCollision(GameObject collider)
    {
        // If the shell touches a building
        if (collider.transform.CompareTag("Shell") && gameObject != null)
        {
            HitByShell(collider);
        }
    }

    private void HitByShell(GameObject collider)
    {
        Destroy(collider);
    }
}
