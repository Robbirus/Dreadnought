using UnityEngine;

[CreateAssetMenu(fileName = "GameMusicContainer", menuName = "Game/Audio/Game Music Data")]
public class GameMusicContainerSO : ScriptableObject
{
    public AudioClip menuMusic;
    public AudioClip tutoMusic;
    public AudioClip combatMusic;
    public AudioClip bossMusic;
    public AudioClip levelUpMusic;
    public AudioClip gameOverMusic;
}
