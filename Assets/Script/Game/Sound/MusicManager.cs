using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;

    [Header("Audio Clip")]
    public AudioClip backgroundMusic;
    public AudioClip levelUpMusic;
    public AudioClip menuMusic;
    public AudioClip gameOverMusic;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }


    public void PlayMusic(AudioClip clip)
    {
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    #region Play Specific Music Methods
    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayLevelUp()
    {
        PlayMusic(levelUpMusic);
    }

    public void PlayBackgroundMusic()
    {
        PlayMusic(backgroundMusic);
    }

    public void PlayGameOverMusic()
    {
        PlayMusic(gameOverMusic);
    }
    #endregion 

    private void TransitionBetweenMusic(AudioClip musicPlayed, AudioClip musicToPlay)
    {

    }
}
