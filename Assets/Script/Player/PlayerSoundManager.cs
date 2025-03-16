using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class PlayerSoundManager : MonoBehaviour
{
    public static PlayerSoundManager instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource movementAudio;
    [SerializeField] private AudioSource actionAudio;

    [Header("Audio Clip")]
    public AudioClip movementSound;
    public AudioClip gunShotSound;
    public AudioClip reloadSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    #region action Sound
    public void PlayGunShot()
    {
        actionAudio.PlayOneShot(gunShotSound);
    }
    public void PlayReload()
    {
        actionAudio.PlayOneShot(reloadSound);
    }
    #endregion

    #region movement Sound
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
}
