using UnityEngine;

public class CollectableTriggerHandler : MonoBehaviour
{
    [SerializeField]
    private Collectable collectable;

    private void Awake()
    {
        collectable = GetComponent<Collectable>();    
    }

    private void OnTriggerEnter(Collider collider)
    {
        ProcessCollision(collider.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ProcessCollision(collision.gameObject);
    }

    private void ProcessCollision(GameObject collision)
    {
        // If the collectable touches the player
        if (collision.transform.CompareTag("Player") && gameObject != null)
        {
            ContactWithPlayer(collision);
        }
    }
    private void ContactWithPlayer(GameObject collider)
    {
        collectable.Collect(collider.gameObject);

        Destroy(gameObject);
    }
}
