using UnityEngine;

public class CollectableSoundManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource collectableSource;

    [Header("Audio Clip")]
    public AudioClip collectSound;

    public void PlayCollectSound()
    {
        collectableSource.PlayOneShot(collectSound);
    }
}
