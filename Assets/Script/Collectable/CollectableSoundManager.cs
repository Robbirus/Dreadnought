using UnityEngine;

public class CollectableSoundManager : MonoBehaviour
{
    public void Play(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}
