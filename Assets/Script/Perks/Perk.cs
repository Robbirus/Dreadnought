using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Perk : MonoBehaviour
{
    [Header("Perk Renderers")]
    [SerializeField] private Image perkImageRenderer;
    [SerializeField] private TMP_Text perkTextRenderer;

    private PerkSO perkInfo;

    public void Setup(PerkSO perk)
    {
        perkInfo = perk;
        perkImageRenderer.sprite = perk.perkImage;
        perkTextRenderer.text = perk.perkText;
    }

    public void SelectedPerk()
    {
        UpgradeManager.instance.SelectPerk(perkInfo);
    }
}

