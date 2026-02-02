using UnityEngine;

[CreateAssetMenu(menuName ="Collectibles/Collectable")]
public class CollectableSO : ScriptableObject
{
    public CollectableType type;
    public float value;
    [Header("Audio Clip")]
    [Tooltip("The audio that will be played when collected")]
    public AudioClip collectSound;
}

public enum CollectableType
{
    Health,
    XP,
    Armor
}
