using UnityEngine;

[CreateAssetMenu(fileName = "AudioContainer", menuName = "Game/Audio/Player Sound Data")]
public class PlayerAudioContainerSO : ScriptableObject
{
    public AudioClip shoot;
    public AudioClip shootCrit;
    public AudioClip reload;
    public AudioClip empty;
}
