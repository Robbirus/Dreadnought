using UnityEngine;


[CreateAssetMenu(fileName = "new Perk", menuName = "Perks/Experience Increase")]
public class ExperienceIncreasePerk : PerkSO
{
    public override void Apply(PlayerController player)
    {
        int xpBonus = player.GetXpManager().GetBonus();

        if (isPercentage)
        {
            xpBonus += xpBonus * (int)(effectValue / 100f);
        }
        else
        {
            xpBonus += (int)effectValue;
        }

        player.GetXpManager().SetBonus(xpBonus);
    }
}
