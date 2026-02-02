using UnityEngine;

public class HealthDrop : MonoBehaviour, ICollectable
{
    [SerializeField] private float healAmount = 100f;

    public void Collect(GameObject collector)
    {
        PlayerHealthManager health = collector.GetComponent<PlayerHealthManager>();
        if (health == null) return;

        health.RestoreHealth(healAmount);
        Destroy(gameObject);
    }
}

