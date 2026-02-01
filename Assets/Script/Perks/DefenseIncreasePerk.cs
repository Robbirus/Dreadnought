using UnityEngine;

[CreateAssetMenu(fileName = "new Perk", menuName = "Perks/Defense Increase")]
public class DefenseIncreasePerk : PerkSO
{
    public override void Apply(PlayerController player)
    {
        int armor = player.GetHealthManager().GetArmor();

        if (isPercentage)
        {
            armor += armor * (int)(effectValue / 100);
        }
        else
        {
            armor += (int)effectValue;
        }

        player.GetHealthManager().SetArmor(armor);
    }
}
