using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Perk", menuName = "Perk")]
public class Upgrade : ScriptableObject
{
    public Sprite image; // The Image of the Perk
    public string description; // The perk's description
    public PerkEffect effectType; // The effect
    public float effectValue; // The value of the effect (10% or +10)
    public bool isUnique; // If unique, the perk will not be randomized again if it's already selected
    public int unlockLevel; 
}

public enum PerkEffect
{
    ReloadDecrease,
    DamageIncrease,
    HealthIncrease,
    SpeedIncrease
}
