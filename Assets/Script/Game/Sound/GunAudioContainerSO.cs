using UnityEngine;

[CreateAssetMenu(fileName = "AudioContainer", menuName = "Game/Audio/Gun Sound Data")]
public class GunAudioContainerSO : ScriptableObject
{
    public AudioClip shoot;
    public AudioClip shootCrit;
    public AudioClip reload;
    public AudioClip empty;
}
