using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    void Start()
    {
        if (GameManager.instance.GetPlayerController() == null)
        {
            Instantiate(playerPrefab, transform.position, transform.rotation);
        }
    }
}
