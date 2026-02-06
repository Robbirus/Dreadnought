using UnityEngine;

public class CollectableSoundManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource collectableSource;

    private void Awake()
    {
        if (collectableSource == null)
            collectableSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        GameObject temp = new GameObject("CollectSound");
        collectableSource = temp.AddComponent<AudioSource>();
        collectableSource.spatialBlend = 0f;
        collectableSource.volume = 0.6f;
        collectableSource.PlayOneShot(clip);
        Destroy(temp, clip.length);
    }
}
