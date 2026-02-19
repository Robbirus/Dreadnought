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
    public AudioClip gunShotSound;
    public AudioClip gunShotCritSound;
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

    public void PlayGunShot()
    {
        actionAudio.PlayOneShot(gunShotSound);
    }

    public void PlayGunShotCrit()
    {
        actionAudio.PlayOneShot(gunShotCritSound);
    }

    public void PlayReload()
    {
        actionAudio.PlayOneShot(reloadSound);
    }
}
