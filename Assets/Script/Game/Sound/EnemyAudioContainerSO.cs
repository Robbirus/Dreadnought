using UnityEngine;

[CreateAssetMenu(fileName = "AudioContainer", menuName = "Game/Audio/Enemy Sound Data")]
public class EnemyAudioContainerSO : ScriptableObject
{
    public AudioClip hitSound;
    public AudioClip hitCritSound;
    public AudioClip deathSound;
}
