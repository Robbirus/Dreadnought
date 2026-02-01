using UnityEngine;

[CreateAssetMenu(fileName = "new Perk", menuName = "Perks/Top Speed")]
public class TopSpeedPerk : PerkSO
{
    public override void Apply(PlayerController player)
    {
        float speed = player.GetMovement().GetMaxSpeed();

        if (isPercentage)
        {
            speed += speed * (effectValue / 100f);
        }
        else
        {
            speed += effectValue;
        }

        player.GetMovement().SetMaxSpeed(speed);
    }
}
