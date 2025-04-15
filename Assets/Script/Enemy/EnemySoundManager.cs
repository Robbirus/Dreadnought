using UnityEngine;

[RequireComponent (typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class EnemySoundManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource movementAudio;
    [SerializeField] private AudioSource actionAudio;

    [Header("Audio Clip")]
    [Tooltip("The sound made by the enemy when it moves")]
    public AudioClip movementSound;
    [Tooltip("The sound made by the enemy when it receives a shell")]
    public AudioClip hitSound;
    [Tooltip("The sound made by the enemy when it receives a critical hit")]
    public AudioClip hitCritSound;
    [Tooltip("The sound made by the enemy when it dies")]
    public AudioClip deathSound;

    private void Start()
    {
        // PlayMovement();
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
    public void PlayHitCritSound()
    {
        actionAudio.PlayOneShot(hitCritSound);
    }

    public void PlayDeathSound()
    {
        actionAudio.PlayOneShot(deathSound);
    }
    #endregion
}
