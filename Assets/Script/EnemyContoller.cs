using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using System;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float ennemySpeed = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        transform.Translate(0, 0, ennemySpeed * Time.deltaTime);
        Quaternion orientation = Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = orientation;
    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log($"Trigger enter started on gameObject {gameObject.name}");
        if (collision.tag.Equals("Player") || collision.tag.Equals("Shell"))
        {
            Destroy(gameObject);
        }
    }
}
