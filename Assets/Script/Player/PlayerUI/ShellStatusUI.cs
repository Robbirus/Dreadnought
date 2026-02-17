using UnityEngine;
using UnityEngine.UI;

public class ShellStatusUI : MonoBehaviour
{
    [Header("HUD")]
    [Tooltip("The frame where the shell image will be")]
    [SerializeField] private Image currentShellImage;

    private GunManager gunManager;

    private void Start()
    {
        gunManager = GameManager.instance
            .GetPlayerController()
            .GetGunManager();

        gunManager.OnShellChanged += UpdateShell;
    }

    private void OnDestroy()
    {
        gunManager.OnShellChanged -= UpdateShell;
    }

    private void UpdateShell(ShellSO shell)
    {
        currentShellImage.sprite = shell.shellImage;
    }
}
