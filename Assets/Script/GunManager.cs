using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunManager : MonoBehaviour
{
    [SerializeField]
    private float shellSpeed = 200f;
    [SerializeField]
    private GameObject shellPrefab;
    [SerializeField]
    private GameObject shellSpawnPoint;

    private float damage = 110f;
    private float fireRate = 15f;

    private int maxAmmo = 1;
    private int currentAmmo;
    private float reloadTime = 4f;
    private bool isReloading = false;

    private float nextTimeToFire = 0f;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0) 
        {
            StartCoroutine(Reload());
        }

        if(Input.GetKey(KeyCode.Space) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }


    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    private void Shoot()
    {
        currentAmmo--;

        GameObject projectile = Instantiate(shellPrefab, shellSpawnPoint.transform.position, shellSpawnPoint.transform.rotation);
        projectile.GetComponent<Rigidbody>().linearVelocity = projectile.transform.forward * shellSpeed;
        
        Destroy(projectile, 3f);
    }
}
