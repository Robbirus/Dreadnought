using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    [Header("Health Properties")]
    [SerializeField]
    private float maxHealth = 1950f;
    [SerializeField]
    private int armor = 10;

    [Header("GameObject Player")]
    [SerializeField]
    private float chipSpeed = 2f;

    [Header("Health Bar UI")]
    [SerializeField]
    private Image frontHealthBar;
    [SerializeField]
    private Image backHealthBar;

    private float health;
    private float lerpTimer;

    #region BloodBath
    private bool bloodbathObtained = false;
    private bool isBloodbath = false;
    private float bloodbathTime = 4f;
    #endregion

    #region Life Rip
    private float lifeRip = 0;
    private bool lifeRipObtained = false;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth/2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // UI
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        // Debug Key heal / damage
        if (Input.GetKey(KeyCode.X))
        {
            TakeDamage(Random.Range(5, 10));
        }
        if (Input.GetKey(KeyCode.V))
        {
            RestoreHealth(Random.Range(5, 10));
        }

        if (isBloodbath && bloodbathObtained)
        {
            if (bloodbathTime > 0)
            {
                RestoreHealth(90f);
                bloodbathTime--;
            } 
            else
            {
                bloodbathTime = 4f;
                isBloodbath = false;
            }
        }
    }

    public void UpdateHealthUI()
    {
        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = health / maxHealth;

        if (fillBack > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.fixedDeltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }

        if (fillFront < healthFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = healthFraction;
            lerpTimer += Time.fixedDeltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        if(damage <= armor)
        {
            health -= 1;
        }
        else
        {
            health -= (damage - armor);
        }
        lerpTimer = 0f;
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }

    public void SetHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public void SetArmor(int armor)
    {
        this.armor = armor;
    }

    public int GetArmor()
    {
        return this.armor;
    }

    public void SetBloodbath(bool isBloodbath)
    {
        this.isBloodbath = isBloodbath;
    }

    public void SetBloodbathObtained(bool bloodbathObtained)
    {
        this.bloodbathObtained = bloodbathObtained;
    }
  
    public void SetLifeRip(float lifeRip)
    {
        this.lifeRip = lifeRip;
    }

    public float GetLifeRip()
    {
        return this.lifeRip;
    }

    public void SetLifeRipObtained(bool lifeRipObtained)
    {
        this.lifeRipObtained = lifeRipObtained;
    }

    public bool IsLifeRipObtained()
    {
        return this.lifeRipObtained;
    }

}
