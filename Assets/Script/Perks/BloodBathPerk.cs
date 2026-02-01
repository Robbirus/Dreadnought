using UnityEngine;

[CreateAssetMenu(fileName = "new Perk", menuName = "Perks/Bloodbath")]
public class BloodBathPerk : PerkSO
{
    public override void Apply(PlayerController player)
    {
        player.GetHealthManager().SetBloodbathObtained(true);
        isUnique = true;
    }
}
