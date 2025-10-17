using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputs playerinputs { get; private set; }
    public PlayerInputs.PlayerActions PlayerActions { get; private set; }

    private void Awake()
    {
        playerinputs = new PlayerInputs();
        PlayerActions = playerinputs.Player;
    }

    private void OnEnable()
    {
        playerinputs.Enable();
    }

    private void OnDisable()
    {
        playerinputs.Disable();
    }
}