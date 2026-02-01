using UnityEngine;

public abstract class PerkSO : ScriptableObject
{
    public Sprite perkImage;
    public string perkText;
    public float effectValue;
    public int unlockLevel;
    public bool isPercentage;
    public bool isUnique;

    public abstract void Apply(PlayerController player);
}

