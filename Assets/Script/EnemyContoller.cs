using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public GameObject player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 direction = Vector3.MoveTowards(gameObject.transform.position, player.transform.position, 0.05f);
        Quaternion orientation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);

        transform.position = direction;
        transform.rotation = orientation;
    }

}
