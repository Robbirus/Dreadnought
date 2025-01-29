using Unity.VisualScripting;
using UnityEngine;

public class HealOnCollision : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject healCollectible;
    private void OnTriggerEnter(Collider other)
    {
        player.GetComponent<PlayerHealthManager>().RestoreHealth(50.0f) ;
        Destroy(healCollectible);

    }
}
