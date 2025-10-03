using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInput playerinputs { get; private set; }
    public PlayerInput.PlayerActions PlayerActions { get; private set; }

    private void Awake()
    {
        playerinputs = new PlayerInput();
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