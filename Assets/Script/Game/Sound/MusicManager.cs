using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;

    [Header("Audio Clip")]
    [SerializeField] private GameMusicContainerSO musics;

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
        PlayMusic(musics.menuMusic);
    }

    public void PlayLevelUp()
    {
        PlayMusic(musics.levelUpMusic);
    }

    public void PlayCombatMusic()
    {
        PlayMusic(musics.combatMusic);
    }

    public void PlayGameOverMusic()
    {
        PlayMusic(musics.gameOverMusic);
    }
    #endregion 

}
