using UnityEngine;

[CreateAssetMenu(fileName = "new Perk", menuName = "Perks/Crits Chance")]
public class CritsChancePerk : PerkSO
{
    public override void Apply(PlayerController player)
    {
        int critChance = player.GetGunManager().GetCritChance();

        if (isPercentage)
        {
            critChance += critChance * (int)(effectValue / 100f);
        }
        else
        {
            critChance += (int)effectValue;
        }

        player.GetGunManager().SetCritChance(critChance);
    }
}
