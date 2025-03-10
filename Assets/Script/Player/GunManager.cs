using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunManager : MonoBehaviour
{
    private const string ANIMATOR_IS_SHOOTING = "isShooting";

    [Header("Animation")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private ParticleSystem shootParticle;

    [Header("Shell Propreties")]
    [SerializeField]
    private GameObject shellPrefab;
    [SerializeField]
    private GameObject shellSpawnPoint;
    [SerializeField]
    private float damage = 110f;
    [SerializeField]
    private int critChance = 2;
    [SerializeField]
    private float critCoef = 1.01f;

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
            yield return new WaitUntil(() => Input.GetKey(KeyCode.Space));
            Shoot();

            // Wait For ReloadTime
            Debug.Log("Reloading");
            Debug.Log(reloadTime);
            AudioManager.instance.PlaySE(AudioManager.instance.gunReloadSE);
            yield return new WaitForSeconds(reloadTime);
            Debug.Log("Ready to shoot");
        }
    }

    private void Shoot()
    {
        Debug.Log("Shoot");
        AudioManager.instance.PlaySE(AudioManager.instance.gunFireSE);
        Instantiate(shellPrefab, shellSpawnPoint.transform.position, shellSpawnPoint.transform.rotation);
        shootParticle.Play();
    }

    public float GetReloadTime()
    {
        return reloadTime;
    }
    public void SetReloadTime(float reloadTime)
    {
        this.reloadTime = reloadTime;
    }

    public float GetDamage()
    {
        return this.damage;
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    public int GetCritChance()
    {
        return this.critChance;
    }
    public void SetCritChance(int critChance)
    {
        this.critChance = critChance;
    }
    public float GetCritCoef()
    {
        return this.critCoef;
    }
    public void SetCritCoef(float critCoef)
    {
        this.critCoef = critCoef;
    }
}
