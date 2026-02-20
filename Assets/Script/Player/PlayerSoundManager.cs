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
    [SerializeField] private GunAudioContainerSO gunAudioContainer;

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
        actionAudio.PlayOneShot(gunAudioContainer.shoot);
    }

    public void PlayGunShotCrit()
    {
        actionAudio.PlayOneShot(gunAudioContainer.shootCrit);
    }

    public void PlayReload()
    {
        actionAudio.PlayOneShot(gunAudioContainer.reload);
    }
}
