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
            Debug.Log("No PlayerController found on collector");
            return;
        }

        Debug.Log("player isn't null : " + player);


        if (soundManager != null && data.collectSound != null)
        {
            soundManager.Play(data.collectSound);
        }

        Destroy(gameObject);
    }
}
