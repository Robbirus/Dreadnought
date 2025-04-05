using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GunManager : MonoBehaviour
{
    private const string ANIMATOR_IS_SHOOTING = "isShooting";

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem shootParticle;
    [Space(10)]

    [Header("Shell Propreties")]
    [Tooltip("The projectile prefab")]
    [SerializeField] private GameObject shellPrefab;
    [Tooltip("The point where the projectile will be launched")]
    [SerializeField] private GameObject shellSpawnPoint;
    [Tooltip("The base damage the projectile will do")]
    [SerializeField] private float damage = 425f;
    [Tooltip("The base chance of doing a critical damage")]
    [SerializeField] private int critChance = 2;
    [Tooltip("The base critical damage coefficient")]
    [SerializeField] private float critCoef = 1.1f;
    [Tooltip("The pity allows to temporarily up the crit chance")]
    [SerializeField] private int pity = 0;
    [Space(10)]

    [Header("Input Action Reference")]
    [SerializeField] private InputActionReference shootActionReference;
    [Space(10)]

    [Header("Reload Properties")]
    [Tooltip("Reload Time in seconds")]
    [SerializeField] private float reloadTime = 4f;
    [Tooltip("The reload circle image")]
    [SerializeField] private Image reloadCircle;

    private void Awake()
    {
        shootActionReference.action.Enable();
    }

    private void Start()
    {
        reloadCircle.enabled = true;
        StartCoroutine(Shooting());
    }
    
    IEnumerator Shooting()
    {
        while (true)
        {
            // Wait for Input Shoot
            yield return new WaitUntil(() => shootActionReference.action.IsPressed());
            Shoot();

            // Wait For ReloadTime
            PlayerSoundManager.instance.PlayReload();

            float elapsed = 0f;
            reloadCircle.fillAmount = 0;

            while (elapsed < reloadTime) 
            {
                elapsed += Time.deltaTime;
                reloadCircle.fillAmount = Mathf.Clamp01(elapsed / reloadTime);
                yield return null;
            }
            Debug.Log("Ready to shoot");
        }
    }

    private void Shoot()
    {
        PlayerSoundManager.instance.PlayGunShot();
        Instantiate(shellPrefab, shellSpawnPoint.transform.position, shellSpawnPoint.transform.rotation);
        shootParticle.Play();
        Debug.Log("Damage : " + damage);
    }

    #region Getter / Setter
   public int GetPity()
   {
        return this.pity;
   }

    public void ResetPity()
    {
        this.pity = 0;
    }
    public void IncreasePity()
    {
        this.pity++;
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
        shellPrefab.GetComponent<Shell>().SetCritChance(this.critChance);
    }

    public float GetCritCoef()
    {
        return this.critCoef;
    }

    public void SetCritCoef(float critCoef)
    {
        this.critCoef = critCoef;
    }
    #endregion
}
