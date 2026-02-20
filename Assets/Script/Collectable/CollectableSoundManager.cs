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

    public void Play(CollectableSO data)
    {
        GameObject temp = new GameObject("CollectSound");
        collectableSource = temp.AddComponent<AudioSource>();

        collectableSource.spatialBlend = data.spatialBlend;
        collectableSource.volume = data.volume;
        collectableSource.pitch = data.pitch;

        collectableSource.PlayOneShot(data.collectSound);

        Destroy(temp, data.collectSound.length);
    }
}
