using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunManager : MonoBehaviour
{
    private const string ANIMATOR_IS_SHOOTING = "isShooting";

    [Header("Animator")]
    [SerializeField]
    private Animator animator;

    [Header("Shell Propreties")]
    [SerializeField]
    private GameObject shellPrefab;
    [SerializeField]
    private GameObject shellSpawnPoint;

    [Header("Input Action Reference")]
    [SerializeField]
    private InputActionReference shootActionReference;

    private float reloadTime = 4f;


    private void Start()
    {
        StartCoroutine(Shooting());
    }

    
    IEnumerator Shooting()
    {
        while (true)
        {
            // Wait for Input Shoot
            yield return new WaitUntil(() => shootActionReference.action.IsPressed());
            //animator.SetBool(ANIMATOR_IS_SHOOTING, );
            Shoot();

            // Wait For ReloadTime
            yield return new WaitForSeconds(reloadTime);
        }
    }


    private void Shoot()
    {
        Instantiate(shellPrefab, shellSpawnPoint.transform.position, shellSpawnPoint.transform.rotation);
    }
}
