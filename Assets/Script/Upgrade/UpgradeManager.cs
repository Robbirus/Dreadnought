using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject perkSelectionUI;
    [SerializeField]
    private GameObject perkPrefab;
    [SerializeField]
    private Transform perkPositionOne;
    [SerializeField]
    private Transform perkPositionTwo;
    [SerializeField]
    private Transform perkPositionThree;
    [SerializeField]
    private List<Upgrade> deck;

    // Currently randomized perks
    private GameObject perkOne;
    private GameObject perkTwo;
    private GameObject perkThree;

    private List<Upgrade> alreadySelectedPerks = new List<Upgrade>();

    public static UpgradeManager instance = null;

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

        List<Upgrade> randomizedPerks = new List<Upgrade>();

        List<Upgrade> availablePerks = new List<Upgrade>(deck);
        availablePerks.RemoveAll(perk =>
            perk.isUnique && alreadySelectedPerks.Contains(perk) ||
            perk.unlockLevel > GameManager.instance.GetCurrentLevel()
        );

        if(availablePerks.Count < 3)
        {
            Debug.Log("Not Enough Available Perks");
            return;
        }

        while(randomizedPerks.Count < 3)
        {
            Upgrade randomPerk = availablePerks[Random.Range(0, availablePerks.Count)];
            if (!randomizedPerks.Contains(randomPerk))
            {
                randomizedPerks.Add(randomPerk); 
            }
        }

        perkOne = InstantiatePerk(randomizedPerks[0], perkPositionOne);
        perkTwo = InstantiatePerk(randomizedPerks[1], perkPositionTwo);
        perkThree = InstantiatePerk(randomizedPerks[2], perkPositionThree);
    }

    private GameObject InstantiatePerk(Upgrade perkSO, Transform position)
    {
        GameObject perkGO = Instantiate(perkPrefab, position.position, Quaternion.identity, position);
        Perk perk = perkGO.GetComponent<Perk>();
        perk.Setup(perkSO);
        return perkGO;
    }

    public void SelectPerk(Upgrade selectedPerk)
    {
        if (!alreadySelectedPerks.Contains(selectedPerk))
        {
            alreadySelectedPerks.Add(selectedPerk);
            ApplyEffect(selectedPerk);
        }

        GameManager.instance.ChangeState(GameManager.GameState.Playing);
    }

    private void ApplyEffect(Upgrade selectedPerk)
    {
        switch (selectedPerk.effectType)
        {
            case PerkEffect.HealthIncrease:
                float maxHealth = GameManager.instance.GetPlayerHealthManager().GetHealth();
                maxHealth = maxHealth * selectedPerk.effectValue;
                GameManager.instance.GetPlayerHealthManager().SetHealth(maxHealth);
                break;

            case PerkEffect.ReloadDecrease:
                float reloadTime = GameManager.instance.GetGunManager().GetReloadTime();
                reloadTime = reloadTime * selectedPerk.effectValue;
                GameManager.instance.GetGunManager().SetReloadTime(reloadTime);
                break;

            case PerkEffect.DamageIncrease:
                break;

            case PerkEffect.SpeedIncrease:
                float maxSpeed = GameManager.instance.GetTankController().GetMaxSpeed();
                maxSpeed += selectedPerk.effectValue;
                GameManager.instance.GetTankController().SetMaxSpeed(maxSpeed);
                break;
        }
    }

    public void ShowPerkSelection()
    {
        perkSelectionUI.SetActive(true); 
        AudioManager.instance.musicSource.clip = AudioManager.instance.levelUpMusic;
        AudioManager.instance.musicSource.Play();
        Time.timeScale = 0f;
    }

    public void HidePerkSelection()
    {
        perkSelectionUI.SetActive(false);
        AudioManager.instance.musicSource.clip = AudioManager.instance.combatMusic;
        AudioManager.instance.musicSource.Play();
        Time.timeScale = 1f;
    }
}
