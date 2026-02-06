using System;
using UnityEngine;

public class SpeedometerUI : MonoBehaviour
{
    [SerializeField] private RectTransform needle;
    [SerializeField] private AnimationCurve speedToAngle;

    private float angle;
    private PlayerMovement playerMovement;

    private void OnEnable()
    {
        if(GameManager.instance.GetPlayerController() != null)
        {
            RegisterPlayer(GameManager.instance.GetPlayerController());
        }

        GameManager.instance.OnPlayerRegisterd += RegisterPlayer;
    }


    private void OnDisable()
    {
        GameManager.instance.OnPlayerRegisterd -= RegisterPlayer;
    }

    private void RegisterPlayer(PlayerController playerController)
    {
        playerMovement = playerController.GetMovement();
    }

    private void Update()
    {
        if (playerMovement == null) return;
        
        angle = speedToAngle.Evaluate(Mathf.Abs(playerMovement.GetCurrentSpeed()));
        needle.localEulerAngles = new Vector3(0, 0, angle);
    }
}
