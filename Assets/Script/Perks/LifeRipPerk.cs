using UnityEngine;

[CreateAssetMenu(fileName = "new Perk", menuName = "Perks/Liferip")]
public class LifeRipPerk : PerkSO
{
    [Header("Life Rip attributes")]
    [Tooltip("Max's life rip stacks")]
    [SerializeField] private int maxStacks = 3;

    private int currentStacks = 0;

    public override void Apply(PlayerController player)
    {
        if(currentStacks >= maxStacks)
        {
            isUnique = true;
            return;
        }

        currentStacks++;

        player.GetHealthManager().SetLifeRipObtained(true);
        float lifeRip = player.GetHealthManager().GetLifeRip();
        lifeRip += effectValue;
        player.GetHealthManager().SetLifeRip(lifeRip);
    }
}
