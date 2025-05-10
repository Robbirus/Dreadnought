using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class ResetDeviceBindings : MonoBehaviour
{
    [Header("Reseting Parameters")]
    [Tooltip("")]
    [SerializeField] private InputActionAsset inputActions;

    [Tooltip("Control Scheme")]
    [SerializeField] private string targetControlScheme;

    public void ResetAllBindings()
    {
        foreach (InputActionMap map in inputActions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
    }

    public void ResetControlSchemeBinding()
    {
        foreach (InputActionMap map in inputActions.actionMaps)
        {
            foreach (InputAction action in map.actions)
            {
                action.RemoveBindingOverride(InputBinding.MaskByGroup(targetControlScheme));
            }
        }
    }
}
