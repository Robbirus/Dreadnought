using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.ShadowCascadeGUI;

public class PerkSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject perkPrefab;
    [SerializeField] private Transform perkPositionOne;
    [SerializeField] private Transform perkPositionTwo;
    [SerializeField] private Transform perkPositionThree;

    private GameObject p1;
    private GameObject p2;
    private GameObject p3;

    private void OnEnable()
    {
        GameManager.instance.RegisterPerksUI(this);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        ShowPerks();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        Clear();
    }

    public void ShowPerks()
    {
        Clear();

        List<PerkSO> perks = UpgradeManager.instance.GetRandomPerks();
        if (perks == null) return;

        p1 = CreatePerk(perks[0], perkPositionOne);
        p2 = CreatePerk(perks[1], perkPositionTwo);
        p3 = CreatePerk(perks[2], perkPositionThree);
    }

    private GameObject CreatePerk(PerkSO perk, Transform parent)
    {
        GameObject go = Instantiate(perkPrefab, parent);
        go.GetComponent<Perk>().Setup(perk);
        return go;
    }

    private void Clear()
    {
        if (p1 != null) Destroy(p1);
        if (p2 != null) Destroy(p2);
        if (p3 != null) Destroy(p3);
    }
}
