using UnityEngine;

[CreateAssetMenu(fileName = "new Perk", menuName = "Perks System/Perk")]
public class PerkSO : ScriptableObject
{
    public Sprite perkImage;
    public PerkEffect effectType;
    public string perkText;
    public float effectValue;
    public int unlockLevel;
    public bool isPercentage;
    public bool isUnique;
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
    LifeRip,
    IncreaseCaliber
}
