using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class Collectable : MonoBehaviour, ICollectable
{
    [SerializeField] private CollectableSO data;
    [SerializeField] private CollectableSoundManager soundManager;

    public void Collect(GameObject collector)
    {
        PlayerController player = collector.GetComponent<PlayerController>();
        if (player == null)
        {
            return;
        }

        if (soundManager != null && data.collectSound != null)
        {
            soundManager.Play(data.collectSound);
        }

        data.Apply(player);

        Destroy(gameObject);
    }
}
