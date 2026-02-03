using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Collectable/Heal")]
public class HealCollectableSO : CollectableSO
{
    public float healAmount;

    public override void Apply(PlayerController player)
    {
        player.GetHealthManager().RestoreHealth(healAmount);
    }
}
