using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("HUD")]
    [Tooltip("The frame where the shell image will be")]
    [SerializeField] private Image currentShellImage;
    [Tooltip("The reload Time display text")]
    [SerializeField] private TMP_Text reloadTimeText;

    [Header("Shell Propreties")]
    [Tooltip("The projectile prefab")]
    [SerializeField] private GameObject shellPrefab;
    [Tooltip("The point where the projectile will be launched")]
    [SerializeField] private GameObject shellSpawnPoint;
    [Tooltip("The base chance of doing a critical damage")]
    [SerializeField] private int critChance = 2;
    [Tooltip("The base critical damage coefficient")]
    [SerializeField] private float critCoef = 1.1f;
    [Tooltip("The pity allows to temporarily up the crit chance")]
    [SerializeField] private int pity = 0;
    [Tooltip("All the shell type in the turret")]
    [SerializeField] private List<ShellSO> ammo;
    [Space(10)]

    [Header("Input Action Reference")]
    [SerializeField] private InputActionReference shootActionReference;
    [Space(10)]

    [Header("Reload Properties")]
    [Tooltip("Reload Time in seconds")]
    [SerializeField] private float reloadTime = 4f;
    [Tooltip("The reload circle image")]
    [SerializeField] private Image reloadCircle;

    private ShellSO currentShell;

    private void Awake()
    {
        shootActionReference.action.Enable();
    }

    private void Start()
    {
        currentShell = ammo[0];
        currentShellImage.sprite = ammo[0].shellImage;
        reloadTimeText.text = reloadTime.ToString("0.00");
        ChangeReloadTimeColor(true);
        reloadCircle.enabled = true;
        StartCoroutine(Shooting());
    }
    
    IEnumerator Shooting()
    {
        while (true)
        {
            // Wait for Input Shoot
            ChangeReloadTimeColor(true);
            yield return new WaitUntil(() => shootActionReference.action.IsPressed());

            Shoot();

            ChangeReloadTimeColor(false);

            // Wait For ReloadTime
            PlayerSoundManager.instance.PlayReload();

            reloadCircle.fillAmount = 0;
            float elapsed = 0f;

            while (elapsed < reloadTime) 
            {
                elapsed += Time.deltaTime;
                reloadCircle.fillAmount = Mathf.Clamp01(elapsed / reloadTime);
                reloadTimeText.text = elapsed.ToString("0.00");
                yield return null;
            }
        }
    }

    private void ChangeReloadTimeColor(bool done)
    {
        if (!done)
        {
            reloadTimeText.color = Color.red;
        }
        else
        {
            reloadTimeText.color = Color.green;
        }
    }

    private void Update()
    {
        SwitchShell();
    }

    private void SwitchShell()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentShell = ammo[0];
            currentShellImage.sprite = ammo[0].shellImage;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
        { 
            currentShell = ammo[1];
            currentShellImage.sprite = ammo[1].shellImage;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentShell = ammo[2];
            currentShellImage.sprite = ammo[2].shellImage;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentShell = ammo[3];
            currentShellImage.sprite = ammo[3].shellImage;
        }
    }

    private void Shoot()
    {
        if (currentShell == null) return;

        InstantiateShell();

        PlayerSoundManager.instance.PlayGunShot();
        shootParticle.Play();
    }

    private void InstantiateShell()
    {
        GameObject shell = Instantiate(shellPrefab, shellSpawnPoint.transform.position, shellSpawnPoint.transform.rotation);
        shell.GetComponent<Shell>().Setup(currentShell);
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
        return this.currentShell.damage;
    }

    public void SetDamage(float damage)
    {
        this.currentShell.damage = damage;
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
