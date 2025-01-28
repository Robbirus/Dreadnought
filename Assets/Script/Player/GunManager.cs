using System.Collections;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shellPrefab;
    [SerializeField]
    private GameObject shellSpawnPoint;
    [SerializeField]
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
        Instantiate(shellPrefab, shellSpawnPoint.transform.position, shellSpawnPoint.transform.rotation);
    }
}
