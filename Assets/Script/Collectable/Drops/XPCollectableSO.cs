using UnityEngine;

[CreateAssetMenu(fileName = "XP", menuName = "Collectable/XP")]
public class XPCollectableSO : CollectableSO
{
    public int xpAmount;

    public override void Apply(PlayerController player)
    {
        player.GetXpManager().GainExperience(xpAmount);
    }
}
