using UnityEngine;

[RequireComponent (typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class SpawnerSoundManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource factoryLoopSource;
    [SerializeField] private AudioSource spawnSoundSource;

    [Header("Audio Data")]
    [SerializeField] private FactoryDataSoundSO factorySounds;

    private void Start()
    {
        PlayFactory();
    }

    public void PlaySpawnSound()
    {
        spawnSoundSource.PlayOneShot(factorySounds.spawnSound);
    }

    public void PlayFactory()
    {
        factoryLoopSource.clip = factorySounds.factoryLoopSound;
        factoryLoopSource.loop = true;
        factoryLoopSource.Play();
    }
    public void StopFactory()
    {
        factoryLoopSource.Stop();
    }
}
