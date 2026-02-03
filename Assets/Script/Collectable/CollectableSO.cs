using UnityEngine;

public abstract class CollectableSO : ScriptableObject
{
    [Header("Audio Clip")]
    [Tooltip("The audio that will be played when collected")]
    public AudioClip collectSound;

    public abstract void Apply(PlayerController player);
}
