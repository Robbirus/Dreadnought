using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour 
{
    [Header("General Setting")]
    [SerializeField] private bool canUse = false;
    [SerializeField] private MainController menuController;
    [Space(10)]

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue;
    [SerializeField] private Slider volumeSlider = null;
    [Space(10)]

    [Header("Brightness Setting")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [Space(10)]

    [Header("Quality Level Setting")]
    [SerializeField] private TMP_Dropdown qualityDropdrown;
    [Space(10)]

    [Header("Fullscreen Setting")]
    [SerializeField] private Toggle fullScreenToggle;
    [Space(10)]

    [Header("Sensitivity Setting")]
    [SerializeField] private TMP_Text controllerSenTextValue = null;
    [SerializeField] private Slider controllerSenSlider = null;
    [Space(10)]

    [Header("Invert Y Setting")]
    [SerializeField] private Toggle invertYToggle = null;
}
