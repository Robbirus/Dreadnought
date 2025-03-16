using UnityEngine;

[RequireComponent (typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class EnemySoundManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource movementAudio;
    [SerializeField] private AudioSource actionAudio;

    [Header("Audio Clip")]
    public AudioClip movementSound;
    public AudioClip hitSound;
    public AudioClip deathSound;

    private void Start()
    {
        PlayMovement();
    }

    #region Movement Sound
    public void PlayMovement()
    {
        movementAudio.clip = movementSound;
        movementAudio.loop = true;
        movementAudio.Play();
    }

    public void StopMovement()
    {
        movementAudio.Stop();
    }
    #endregion

    #region Action Sound
    public void PlayHitSound()
    {
        actionAudio.PlayOneShot(hitSound);
    }
    public void PlayDeathSound()
    {
        actionAudio.PlayOneShot(deathSound);
    }
    #endregion
}
