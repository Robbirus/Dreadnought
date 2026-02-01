using UnityEngine;

[CreateAssetMenu(fileName = "new Perk", menuName = "Perks/Caliber Increase")]
public class CaliberIncreasePerk : PerkSO
{
    public override void Apply(PlayerController player)
    {
        int caliber = player.GetGunManager().GetCaliber();

        if (isPercentage)
        {
            caliber += caliber * (int)(effectValue / 100f);
        }
        else
        {
            caliber += (int)effectValue;
        }

        player.GetGunManager().SetCaliber(caliber);
    }
}
