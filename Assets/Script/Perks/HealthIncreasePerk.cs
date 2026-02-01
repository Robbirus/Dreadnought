using UnityEngine;

[CreateAssetMenu(fileName = "new Perk", menuName = "Perks/Health Increase")]
public class HealthIncreasePerk : PerkSO
{
    public override void Apply(PlayerController player)
    {
        float life = player.GetHealthManager().GetMaxHealth();
        if (isPercentage)
        {
            life += life * (effectValue / 100f);
        }
        else
        {
            life += effectValue;
        }

        player.GetHealthManager().SetMaxHealth(life);
    }
}
