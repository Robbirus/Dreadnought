using UnityEngine;
using UnityEngine.UI;

public class ShellStatusUI : MonoBehaviour
{
    [Header("HUD")]
    [Tooltip("The frame where the shell image will be")]
    [SerializeField] private Image shellFrame;
    [SerializeField] private Sprite baseShell;

    private GunManager gunManager;

    private void Start()
    {
        gunManager = GameManager.instance
            .GetPlayerController()
            .GetGunManager();

        shellFrame.sprite = baseShell;

        gunManager.OnShellChanged += UpdateShell;
    }

    private void OnDestroy()
    {
        gunManager.OnShellChanged -= UpdateShell;
    }

    private void UpdateShell(ShellSO shell)
    {
        shellFrame.sprite = shell.shellImage;
    }
}
