using UnityEngine;

[RequireComponent (typeof(AudioSource))]
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
        if (collision.transform.parent.gameObject.transform.CompareTag("Player") && gameObject != null)
        {
            ContactWithPlayer(collision);
        }
    }

    private void ContactWithPlayer(GameObject collider)
    {
        gameObject.GetComponent<CollectableSoundManager>().PlayCollectSound();
        collectable.Collect(collider.transform.parent.gameObject);
        Destroy(gameObject);
    }
}
