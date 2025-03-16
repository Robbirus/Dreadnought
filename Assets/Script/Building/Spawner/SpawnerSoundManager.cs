using UnityEngine;

[RequireComponent (typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class SpawnerSoundManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource factoryLoopSource;
    [SerializeField] private AudioSource spawnSoundSource;

    [Header("Audio Clip")]
    public AudioClip factoryLoopSound;
    public AudioClip spawnSound;

    private void Start()
    {
        PlayFactory();
    }

    public void PlaySpawnSound()
    {
        spawnSoundSource.PlayOneShot(spawnSound);
    }

    public void PlayFactory()
    {
        factoryLoopSource.clip = factoryLoopSound;
        factoryLoopSource.loop = true;
        factoryLoopSource.Play();
    }
    public void StopFactory()
    {
        factoryLoopSource.Stop();
    }
}
