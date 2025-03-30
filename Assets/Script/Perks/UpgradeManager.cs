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
        instance = this;

        if (GameManager.instance != null)
        {
            GameManager.instance.OnStateChanged += HandleGameStateChanged;
        }
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
                ApplyTopSpeedIncrease(selectedPerk);
                break;

            case PerkEffect.AccelerationSpeed:
                ApplyAccelerationSpeed(selectedPerk);
                break;

            case PerkEffect.CritsDamageIncrease:
                ApplyCritsDamageIncrease(selectedPerk);
                break;

            case PerkEffect.CritsChanceIncrease:
                ApplyCritsChanceIncrease(selectedPerk);
                break;

            case PerkEffect.DefenseIncrease:
                ApplyDefenseIncrease(selectedPerk);
                break;

            case PerkEffect.ExperienceIncrease:
                ApplyExperienceIncrease(selectedPerk);
                break;

            case PerkEffect.HealthIncrease:
                ApplyHealthIncrease(selectedPerk);
                break;

            case PerkEffect.DamageIncrease:
                ApplyDamageIncrease(selectedPerk);
                break;
    
            case PerkEffect.ReloadDecrease:
                ApplyReloadDecrease(selectedPerk);
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
    private void ApplyTopSpeedIncrease(PerkSO selectedPerk)
    {
        float maxSpeed = GameManager.instance.GetPlayerMovement().GetMaxSpeed();
        float reverseSpeed = GameManager.instance.GetPlayerMovement().GetReverseSpeed();
        maxSpeed += selectedPerk.effectValue;
        reverseSpeed += selectedPerk.effectValue / 2;
        GameManager.instance.GetPlayerMovement().SetMaxSpeed(maxSpeed);
        GameManager.instance.GetPlayerMovement().SetReverseSpeed(reverseSpeed);
    }
   
    private void ApplyAccelerationSpeed(PerkSO selectedPerk)
    {

    }

    private void ApplyCritsDamageIncrease(PerkSO selectedPerk)
    {
        float critCoef = GameManager.instance.GetGunManager().GetCritCoef();
        critCoef *= selectedPerk.effectValue;
        GameManager.instance.GetGunManager().SetCritCoef(critCoef);
    }

    private void ApplyCritsChanceIncrease(PerkSO selectedPerk)
    {
        int critChance = GameManager.instance.GetGunManager().GetCritChance();
        critChance += (int)selectedPerk.effectValue;
        GameManager.instance.GetGunManager().SetCritChance(critChance);
    }

    private void ApplyDefenseIncrease(PerkSO selectedPerk)
    {
        int armor = GameManager.instance.GetPlayerHealthManager().GetArmor();
        armor += (int)selectedPerk.effectValue;
        GameManager.instance.GetPlayerHealthManager().SetArmor(armor);
    }

    private void ApplyExperienceIncrease(PerkSO selectedPerk)
    {
        int bonus = GameManager.instance.GetExperienceManager().GetBonus();
        bonus = (int)selectedPerk.effectValue;
        GameManager.instance.GetExperienceManager().SetBonus(bonus);
    }

    private void ApplyHealthIncrease(PerkSO selectedPerk)
    {
        float maxHealth = GameManager.instance.GetPlayerHealthManager().GetHealth();
        maxHealth *= selectedPerk.effectValue;
        GameManager.instance.GetPlayerHealthManager().SetHealth(maxHealth);
    }
    
    private void ApplyDamageIncrease(PerkSO selectedPerk)
    {
        float damage = GameManager.instance.GetGunManager().GetDamage();
        damage *= selectedPerk.effectValue;
        GameManager.instance.GetGunManager().SetDamage(damage);
    }

    private void ApplyReloadDecrease(PerkSO selectedPerk)
    {
        float reloadTime = GameManager.instance.GetGunManager().GetReloadTime();
        reloadTime *= selectedPerk.effectValue;
        GameManager.instance.GetGunManager().SetReloadTime(reloadTime);
    }

    private void ApplyBloodBath(PerkSO selectedPerk)
    {
        GameManager.instance.GetPlayerHealthManager().SetBloodbathObtained(true);
        selectedPerk.isUnique = true;

    }

    private void ApplyLifeRip(PerkSO selectedPerk)
    {
        int passage = 0;
        if (passage < 3)
        {
            GameManager.instance.GetPlayerHealthManager().SetLifeRipObtained(true);
            float lifeRip = GameManager.instance.GetPlayerHealthManager().GetLifeRip();
            lifeRip += selectedPerk.effectValue;
            GameManager.instance.GetPlayerHealthManager().SetLifeRip(lifeRip);
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
    }

    public void HidePerkSelection()
    {
        perkSelectionUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
