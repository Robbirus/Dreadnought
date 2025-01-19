using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform shellSpawnPoint;
    public GameObject shellPrefab;
    public float shellSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject shell = Instantiate(shellPrefab, shellSpawnPoint.position, shellSpawnPoint.rotation);
            shell.GetComponent<Rigidbody>().linearVelocity = shellSpawnPoint.forward * shellSpeed;

        }
    }
}
