using UnityEngine;

public abstract class CollectableSO : ScriptableObject
{
    [Header("Audio Clip")]
    [Tooltip("The audio that will be played when collected")]
    public AudioClip collectSound;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.5f, 1.5f)] public float pitch = 1f;
    [Range(0f, 1f)] public float spatialBlend = 0f;

    public abstract void Apply(PlayerController player);
}
