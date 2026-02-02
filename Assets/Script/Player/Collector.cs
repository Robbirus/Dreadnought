using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ICollectable collectable = other.GetComponentInParent<ICollectable>();
        if (collectable != null)
        {
            Debug.Log("TRIGGER ENTER with " + other.name);
            collectable.Collect(gameObject);
        }
    }
}
