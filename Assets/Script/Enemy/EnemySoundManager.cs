using UnityEngine;

[RequireComponent (typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class EnemySoundManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource movementAudio;
    [SerializeField] private AudioSource actionAudio;

    [Header("Audio Clip")]
    [SerializeField] private EnemyAudioContainerSO audioData;


    #region Action Sound
    public void PlayHitSound()
    {
        actionAudio.PlayOneShot(audioData.hitSound);
    }
    public void PlayHitCritSound()
    {
        actionAudio.PlayOneShot(audioData.hitCritSound);
    }

    public void PlayDeathSound()
    {
        actionAudio.PlayOneShot(audioData.deathSound);
    }
    #endregion
}
