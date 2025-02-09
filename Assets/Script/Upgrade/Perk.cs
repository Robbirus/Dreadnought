using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Perk : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer perkImageRenderer;
    [SerializeField]
    private TMP_Text perkTextRenderer;

    private Upgrade perkInfo;

    public void Setup(Upgrade perk)
    {
        perkInfo = perk;
        perkImageRenderer.sprite = perk.image; // Something wrong in here
        perkTextRenderer.text = perk.description;
    }

    public void SelectPerk()
    {
        Debug.Log("Clicked");
        UpgradeManager.instance.SelectPerk(perkInfo);
    }

}
