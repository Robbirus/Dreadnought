using System;
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

        ApplyEffect(player);

        if (soundManager != null && data.collectSound != null)
        {
            soundManager.Play(data.collectSound);
        }

        Destroy(gameObject);
    }

    private void ApplyEffect(PlayerController player)
    {
        switch(data.type)
        {
            case CollectableType.Health:
                player.GetHealthManager().RestoreHealth(data.value);
                break;

            case CollectableType.XP:
                player.GetXpManager().GainExperience((int)data.value);
                break;

            case CollectableType.Armor:
                int armor = player.GetHealthManager().GetArmor();
                player.GetHealthManager().SetArmor(armor + (int)data.value);
                break;
        }
    }
}
