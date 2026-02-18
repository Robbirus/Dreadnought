using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunManager : MonoBehaviour
{
    private const string ANIMATOR_IS_SHOOTING = "isShooting";

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem shootParticle;
    [Space(10)]

    [Header("Gun Propreties")]
    [SerializeField] private GunSO currentGun;
    [Tooltip("The projectile prefab")]
    [SerializeField] private GameObject shellPrefab;
    [Tooltip("The point where the projectile will be launched")]
    [SerializeField] private GameObject shellSpawnPoint;
    [Tooltip("The base chance of doing a critical damage")]
    [SerializeField] private int critChance = 2;
    [Tooltip("The base critical damage coefficient")]
    [SerializeField] private float critCoef = 1.1f;
    [Tooltip("The pity allows to temporarily up the crit chance")]
    [Range(0, 100)]
    [SerializeField] private int pity = 0;    
    [Space(10)]

    [Header("Input Action Reference")]
    [SerializeField] private InputActionReference shootActionReference;
    [Space(10)]

    [SerializeField] private InputActionReference ammo1ActionReference;
    [SerializeField] private InputActionReference ammo2ActionReference;
    [SerializeField] private InputActionReference ammo3ActionReference;
    [SerializeField] private InputActionReference ammo4ActionReference;

    #region Events
    public event Action<ShellSO> OnShellChanged;
    public event Action<float, float> OnReloadProgress;
    public event Action<bool> OnReloadStateChanged;
    #endregion

    private ShellSO currentShell;
    private float damage;
    private float reloadTime;
    private int penetration;
    private int caliber; 
    private List<ShellSO> ammo;

    private void Awake()
    {
        Setup(currentGun);
        
        shootActionReference.action.Enable();

        ammo1ActionReference.action.Enable();
        ammo2ActionReference.action.Enable();
        ammo3ActionReference.action.Enable();
        ammo4ActionReference.action.Enable();
    }

    private void Setup(GunSO currentGun)
    {
        this.caliber = currentGun.caliber;
        this.damage = currentGun.damage;
        this.reloadTime = currentGun.reloadTime;
        this.penetration = currentGun.penetration;
        this.ammo = currentGun.ammo;
    }

    private void Start()
    {
        SetShell(0);
        StartCoroutine(Shooting());
    }
    
    IEnumerator Shooting()
    {
        while (true)
        {
            OnReloadStateChanged?.Invoke(true);

            // Wait for Input Shoot
            yield return new WaitUntil(() => shootActionReference.action.IsPressed() && !MenuPause.isGamePaused);

            Shoot();

            OnReloadStateChanged?.Invoke(false);

            // Wait For ReloadTime
            PlayerSoundManager.instance.PlayReload();

            float elapsed = 0f;

            while (elapsed < this.reloadTime)
            {
                elapsed += Time.deltaTime;
                OnReloadProgress?.Invoke(elapsed, this.reloadTime);
                yield return null;
            }
        }
    }

    /// <summary>
    /// Shoot a random number to know if the shot is a crit or not
    /// </summary>
    /// <returns>True if the shot is a crit, false otherwise</returns>
    private bool RollCrit()
    {
        int roll = UnityEngine.Random.Range(0, 100);

        if(roll < critChance + pity)
        {
            ResetPity();
            return true;
        }
        else
        {
            IncreasePity();
            return false;
        }
    }

    private void Update()
    {
        SwitchShell();
    }

    private void SwitchShell()
    {        
        if (ammo1ActionReference.action.IsPressed())
        {
            SetShell(0);
        }
        if (ammo2ActionReference.action.IsPressed()) 
        { 
            SetShell(1);
        }
        if (ammo3ActionReference.action.IsPressed())
        {
            SetShell(2);
        }
        if (ammo4ActionReference.action.IsPressed())
        {
            SetShell(3);
        }
    }

    private void SetShell(int index)
    {
        currentShell = ammo[index];
        OnShellChanged?.Invoke(currentShell);
    }

    private void Shoot()
    {
        if (currentShell == null) return;

        GameManager.instance.IncreaseShotFired();

        bool isCrit = RollCrit();
        InstantiateShell(isCrit);
        
        if (isCrit)
        {
            PlayerSoundManager.instance.PlayGunShotCrit();
        }
        else
        {
            PlayerSoundManager.instance.PlayGunShot();
        }

        shootParticle.Play();
    }

    private void InstantiateShell(bool isCrit)
    {
        GameObject shell = Instantiate(shellPrefab, shellSpawnPoint.transform.position, shellSpawnPoint.transform.rotation);
        shell.GetComponent<Shell>().Setup(currentShell, Team.Player, this.caliber, isCrit, this.penetration, this.damage);
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

    public int GetCaliber()
    {
        return this.caliber;
    }
    public void SetCaliber(int caliber)
    {
        this.caliber = caliber;
    }
    #endregion
}
