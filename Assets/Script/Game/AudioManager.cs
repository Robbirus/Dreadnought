using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField]
    public AudioSource musicSource;
    [SerializeField]
    public AudioSource soundEffectSource;

    [Header("Audio Clip Sound Effect")]
    public AudioClip hitSE;
    public AudioClip gunFireSE;
    public AudioClip gunReloadSE;

    [Header("Audio Clip Music")]
    public AudioClip bossMusic;
    public AudioClip combatMusic;
    public AudioClip invicibleMusic;
    public AudioClip menuMusic;
    public AudioClip defeatMusic;
    public AudioClip levelUpMusic;

    public static AudioManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySE(AudioClip clip)
    {
        soundEffectSource.PlayOneShot(clip);
    }

}
