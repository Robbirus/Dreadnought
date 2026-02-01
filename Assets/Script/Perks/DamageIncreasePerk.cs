using UnityEngine;

[CreateAssetMenu(fileName = "new Perk", menuName = "Perks/Damage Increase")]
public class DamageIncreasePerk : PerkSO
{
    public override void Apply(PlayerController player)
    {
        float damage = player.GetGunManager().GetDamage();

        if (isPercentage)
        {
            damage += damage * (effectValue / 100f);
        }
        else
        {
            damage += effectValue;
        }

        player.GetGunManager().SetDamage(damage);
    }
}
