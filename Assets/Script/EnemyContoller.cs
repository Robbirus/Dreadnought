using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    private GameObject player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 direction = Vector3.MoveTowards(gameObject.transform.position, player.transform.position, 0.1f);
        Quaternion orientation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);

        transform.position = direction;
        transform.rotation = orientation;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log($"Trigger enter started on gameObject {gameObject.name}");
        if (collision.tag.Equals("Player"))
        {
            Destroy(gameObject);
        }
    }
}
