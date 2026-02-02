using UnityEngine;

public class XpDrop : MonoBehaviour, ICollectable
{
    [SerializeField] private int xpAmount = 10;

    public void Collect(GameObject collector)
    {
        PlayerController player = collector.GetComponent<PlayerController>();
        if (player == null) return;

        player.GetXpManager().GainExperience(xpAmount);
        Destroy(gameObject);
    }
}
