using UnityEngine;

[CreateAssetMenu(fileName = "new Perk", menuName = "Perks System/Perk")]
public class PerkSO : ScriptableObject
{
    public Sprite perkImage;
    public string perkText;
    public PerkEffect effectType;
    public float effectValue;
    public bool isUnique;
    public int unlockLevel;
}

public enum PerkEffect
{
    TopSpeedIncrease,
    AccelerationSpeed,
    CritsDamageIncrease,
    CritsChanceIncrease,
    DefenseIncrease,
    ExperienceIncrease,
    HealthIncrease,
    DamageIncrease,
    ReloadDecrease,
    Bloodbath,
    LifeRip
}
