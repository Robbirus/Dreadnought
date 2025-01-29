using Unity.VisualScripting;
using UnityEngine;

public class Script : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        player.GetComponent<PlayerHealthManager>().RestoreHealth(50.0f) ;
        Destroy(gameObject);

    }
}
