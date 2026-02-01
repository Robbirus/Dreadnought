using UnityEngine;

[CreateAssetMenu(fileName = "new Perk", menuName = "Perks/Crits Damage")]
public class CritsDamagePerk : PerkSO
{
    public override void Apply(PlayerController player)
    {
        float crit = player.GetGunManager().GetCritCoef();

        if (isPercentage)
        {
            crit += crit * (effectValue / 100f);
        }
        else
        {
            crit += effectValue;
        }

        player.GetGunManager().SetCritCoef(crit);
    }
}
