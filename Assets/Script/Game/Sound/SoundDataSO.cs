using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Game/Audio/Sound Data")]
public class SoundDataSO : ScriptableObject
{
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.5f, 1.5f)] public float pitch = 1f;
}
