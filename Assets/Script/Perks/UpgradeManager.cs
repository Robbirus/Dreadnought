using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour
{
    [Header("Perk Attributs")]
    [SerializeField] private List<PerkSO> deck;

    public static UpgradeManager instance = null;

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

    public List<PerkSO> GetRandomPerks()
    {
        List<PerkSO> availablePerks = new List<PerkSO>(deck);
        availablePerks.RemoveAll(perk =>
            perk.isUnique || perk.unlockLevel > GameManager.instance.GetCurrentLevel()
        );

        if (availablePerks.Count < 3)
            return null;

        List<PerkSO> result = new();

        while (result.Count < 3)
        {
            PerkSO p = availablePerks[Random.Range(0, availablePerks.Count)];
            if (!result.Contains(p))
                result.Add(p);
        }

        return result;
    }

    public void SelectPerk(PerkSO selectedPerk)
    {
        selectedPerk.Apply(GameManager.instance.GetPlayerController());
        GameManager.instance.ChangeState(GameState.Playing);
    }
}
