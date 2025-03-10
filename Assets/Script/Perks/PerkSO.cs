using UnityEngine;

[CreateAssetMenu(fileName = "new Perk", menuName = "Perk")]
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
    SpeedIncrease,
    DamageIncrease,
    CritsDamageIncrease,
    ReloadDecrease
}
