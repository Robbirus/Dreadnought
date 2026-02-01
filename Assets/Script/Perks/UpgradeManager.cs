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
        selectedPerk.Apply(GameManager.instance.GetPlayerController());
        GameManager.instance.ChangeState(GameManager.GameState.Playing);
    }
    
    private void OnEnable()
    {
        if (GameManager.instance != null)
            GameManager.instance.OnStateChanged += HandleGameStateChanged;
    }

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
