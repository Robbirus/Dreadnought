using UnityEngine;

[CreateAssetMenu(fileName = "FactoryDataSoundSO", menuName = "Game/Audio/Factory Sounds Data")]
public class FactoryDataSoundSO : ScriptableObject
{
    public AudioClip factoryLoopSound;
    public AudioClip spawnSound;
}
