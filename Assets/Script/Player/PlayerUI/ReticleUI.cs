using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReticleUI : MonoBehaviour
{
    [Header("HUD")]
    [Tooltip("The circle image")]
    [SerializeField] private Image reloadCircle;
    [Tooltip("The reload time text")]
    [SerializeField] private TMP_Text reloadTimeText;

    private GunManager gunManager;

    private void Start()
    {
        gunManager = GameManager.instance
            .GetPlayerController()
            .GetGunManager();

        gunManager.OnReloadProgress += UpdateReload;
        gunManager.OnReloadStateChanged += UpdateColor;
    }

    private void OnDestroy()
    {
        gunManager.OnReloadProgress -= UpdateReload;
        gunManager.OnReloadStateChanged -= UpdateColor;
    }

    private void UpdateReload(float currentTime, float max)
    {
        float t = currentTime / max;
        reloadCircle.fillAmount = t;
        reloadTimeText.text = currentTime.ToString("0.0");
    }

    private void UpdateColor(bool isReloading)
    {
        reloadTimeText.color = isReloading ? Color.green : Color.red;
    }   
}
