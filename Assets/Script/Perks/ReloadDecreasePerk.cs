using UnityEngine;

[CreateAssetMenu(fileName = "new Perk", menuName = "Perks/Reload Decrease")]
public class ReloadDecreasePerk : PerkSO
{
    public override void Apply(PlayerController player)
    {
        float reloadTime = player.GetGunManager().GetReloadTime();

        if (isPercentage)
        {
            reloadTime += reloadTime * (effectValue / 100f);        
        }
        else
        {
            reloadTime += effectValue;
        }
    }
}
