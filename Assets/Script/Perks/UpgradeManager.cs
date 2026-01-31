using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("Perk Attributs")]
    [SerializeField] GameObject perkSelectionUI;
    [SerializeField] GameObject perkPrefab;
    [SerializeField] Transform perkPositionOne;
    [SerializeField] Transform perkPositionTwo;
    [SerializeField] Transform perkPositionThree;
    [SerializeField] List<PerkSO> deck;

    // Currently randomized perks
    private GameObject perkOne, perkTwo, perkThree;

    public static UpgradeManager instance = null;

    public void Start()
    {
        //RandomizeNewPerks();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }


    private void OnDisable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.PerkSelection)
        {
            RandomizeNewPerks();
        }
    }
   
    private void RandomizeNewPerks()
    {
        if(perkOne != null) Destroy(perkOne);
        if (perkTwo != null) Destroy(perkTwo);
        if (perkThree != null) Destroy(perkThree);

        List<PerkSO> randomizedPerks = new List<PerkSO>();

        List<PerkSO> availablePerks = new List<PerkSO>(deck);
        availablePerks.RemoveAll(perk =>
            perk.isUnique || perk.unlockLevel > GameManager.instance.GetCurrentLevel()
        );

        if(availablePerks.Count < 3)
        {
            Debug.Log("Not Enough Available Perks");
            return;
        }

        while(randomizedPerks.Count < 3)
        {
            PerkSO randomPerk = availablePerks[Random.Range(0, availablePerks.Count)];
            if (!randomizedPerks.Contains(randomPerk))
            {
                randomizedPerks.Add(randomPerk); 
            }
        }

        perkOne = InstantiatePerk(randomizedPerks[0], perkPositionOne);
        perkTwo = InstantiatePerk(randomizedPerks[1], perkPositionTwo);
        perkThree = InstantiatePerk(randomizedPerks[2], perkPositionThree);
    }

    private GameObject InstantiatePerk(PerkSO perkSO, Transform position)
    {
        GameObject perkGO = Instantiate(perkPrefab, position.position, Quaternion.identity, position);
        Perk perk = perkGO.GetComponent<Perk>();
        perk.Setup(perkSO);
        return perkGO;
    }

    public void SelectPerk(PerkSO selectedPerk)
    {
        ApplyEffect(selectedPerk);
        GameManager.instance.ChangeState(GameManager.GameState.Playing);
    }

    private void ApplyEffect(PerkSO selectedPerk)
    {
        switch (selectedPerk.effectType)
        {
            case PerkEffect.TopSpeedIncrease:
                float speed = ModifyStat(GameManager.instance.GetPlayerController().GetMovement().GetMaxSpeed(), selectedPerk);
                GameManager.instance.GetPlayerController().GetMovement().SetMaxSpeed(speed);
                break;

                // A verifier
            case PerkEffect.CritsDamageIncrease:
                float critDamage = ModifyStat(GameManager.instance.GetPlayerController().GetGunManager().GetCritCoef(), selectedPerk);
                GameManager.instance.GetPlayerController().GetGunManager().SetCritCoef(critDamage);
                break;

                // A verifier
            case PerkEffect.CritsChanceIncrease:
                int critChance = ModifyStat(GameManager.instance.GetPlayerController().GetGunManager().GetCritChance(), selectedPerk);
                GameManager.instance.GetPlayerController().GetGunManager().SetCritChance(critChance);
                break;

            case PerkEffect.DefenseIncrease:
                Debug.Log("Defense Increased");
                break;

            case PerkEffect.ExperienceIncrease:
                int xpBonus = ModifyStat(GameManager.instance.GetPlayerController().GetXpManager().GetBonus(), selectedPerk);
                GameManager.instance.GetPlayerController().GetXpManager().SetBonus(xpBonus);
                break;

            case PerkEffect.HealthIncrease:
                float life = ModifyStat(GameManager.instance.GetPlayerController().GetHealthManager().GetMaxHealth(), selectedPerk);
                GameManager.instance.GetPlayerController().GetHealthManager().SetMaxHealth(life);
                break;

                // N'augmente que l'obus actuellement charger
            case PerkEffect.DamageIncrease:
                float damage = ModifyStat(GameManager.instance.GetPlayerController().GetGunManager().GetDamage(), selectedPerk);
                GameManager.instance.GetPlayerController().GetGunManager().SetDamage(damage);
                break;
    
            case PerkEffect.ReloadDecrease:
                float reloadTime = ModifyStat(GameManager.instance.GetPlayerController().GetGunManager().GetReloadTime(), selectedPerk);
                GameManager.instance.GetPlayerController().GetGunManager().SetReloadTime(reloadTime);
                break;


            case PerkEffect.IncreaseCaliber:
                int caliber = ModifyStat(GameManager.instance.GetPlayerController().GetGunManager().GetCaliber(), selectedPerk);
                GameManager.instance.GetPlayerController().GetGunManager().SetCaliber(caliber);
                break;

            case PerkEffect.Bloodbath:
                ApplyBloodBath(selectedPerk);
                break;

            case PerkEffect.LifeRip:
                ApplyLifeRip(selectedPerk);
                break;

        }
    }

    #region Apply Upgrades  
    private int ModifyStat(int stat, PerkSO perk)
    {
        bool isPercent = perk.isPercentage;

        if (isPercent)
        {
            stat += (int)(stat * (perk.effectValue / 100f));
        }
        else
        {
            stat += (int)perk.effectValue;
        }
        return stat;
    }

    private float ModifyStat(float stat, PerkSO perk)
    {
        bool isPercent = perk.isPercentage;

        if (isPercent)
        {
            stat += (stat * (perk.effectValue / 100f));
        }
        else
        {
            stat += perk.effectValue;
        }
        return stat;
    }

    /*
    private void ApplyTopSpeedIncrease(PerkSO selectedPerk)
    {
        float maxSpeed = GameManager.instance.GetPlayerController().GetMovement().GetMaxSpeed();
        float reverseSpeed = GameManager.instance.GetPlayerController().GetMovement().GetReverseSpeed();
        maxSpeed += selectedPerk.effectValue;
        reverseSpeed += selectedPerk.effectValue / 2;
        GameManager.instance.GetPlayerController().GetMovement().SetMaxSpeed(maxSpeed);
        GameManager.instance.GetPlayerController().GetMovement().SetReverseSpeed(reverseSpeed);
    }
   
    private void ApplyAccelerationSpeed(PerkSO selectedPerk)
    {

    }

    private void ApplyCritsDamageIncrease(PerkSO selectedPerk)
    {
        float critCoef = GameManager.instance.GetPlayerController().GetGunManager().GetCritCoef();
        critCoef *= selectedPerk.effectValue;
        GameManager.instance.GetPlayerController().GetGunManager().SetCritCoef(critCoef);
    }

    private void ApplyCritsChanceIncrease(PerkSO selectedPerk)
    {
        int critChance = GameManager.instance.GetPlayerController().GetGunManager().GetCritChance();
        critChance += (int)selectedPerk.effectValue;
        GameManager.instance.GetPlayerController().GetGunManager().SetCritChance(critChance);
    }

    private void ApplyDefenseIncrease(PerkSO selectedPerk)
    {
        int armor = GameManager.instance.GetPlayerController().GetHealthManager().GetArmor();
        armor += (int)selectedPerk.effectValue;
        GameManager.instance.GetPlayerController().GetHealthManager().SetArmor(armor);
    }

    private void ApplyExperienceIncrease(PerkSO selectedPerk)
    {
        int bonus = GameManager.instance.GetPlayerController().GetXpManager().GetBonus();
        bonus = (int)selectedPerk.effectValue;
        GameManager.instance.GetPlayerController().GetXpManager().SetBonus(bonus);
    }

    private void ApplyHealthIncrease(PerkSO selectedPerk)
    {
        float maxHealth = GameManager.instance.GetPlayerController().GetHealthManager().GetHealth();
        maxHealth *= selectedPerk.effectValue;
        GameManager.instance.GetPlayerController().GetHealthManager().SetHealth(maxHealth);
    }
    
    private void ApplyDamageIncrease(PerkSO selectedPerk)
    {
        float damage = GameManager.instance.GetPlayerController().GetGunManager().GetDamage();
        damage *= selectedPerk.effectValue;
        GameManager.instance.GetPlayerController().GetGunManager().SetDamage(damage);
    }

    private void ApplyReloadDecrease(PerkSO selectedPerk)
    {
        float reloadTime = GameManager.instance.GetPlayerController().GetGunManager().GetReloadTime();
        reloadTime *= selectedPerk.effectValue;
        GameManager.instance.GetPlayerController().GetGunManager().SetReloadTime(reloadTime);
    }
    */

    private void ApplyBloodBath(PerkSO selectedPerk)
    {
        GameManager.instance.GetPlayerController().GetHealthManager().SetBloodbathObtained(true);
        selectedPerk.isUnique = true;
    }

    private void ApplyLifeRip(PerkSO selectedPerk)
    {
        int passage = 0;
        if (passage < 3)
        {
            GameManager.instance.GetPlayerController().GetHealthManager().SetLifeRipObtained(true);
            float lifeRip = GameManager.instance.GetPlayerController().GetHealthManager().GetLifeRip();
            lifeRip += selectedPerk.effectValue;
            GameManager.instance.GetPlayerController().GetHealthManager().SetLifeRip(lifeRip);
            passage++;
        }
        else
        {
            selectedPerk.isUnique = true;
        }
    }
    #endregion

    public void ShowPerkSelection()
    {
        perkSelectionUI.SetActive(true);
        MusicManager.instance.PlayLevelUp();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void HidePerkSelection()
    {
        perkSelectionUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
