using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private PlayerInput _playerInput;

    public event Action ScanPressed;
    public event Action LMBClicked;
    public event Action RMBClicked;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.Game.Scan.performed += OnScanned;
        _playerInput.Game.LeftClick.performed += OnLMBClicked;
        _playerInput.Game.RightClick.performed += OnRMBClicked;
    }

    private void OnEnable() => _playerInput.Enable();
    private void OnDisable() => _playerInput.Disable();

    public void OnScanned(InputAction.CallbackContext context) => ScanPressed?.Invoke();
    public void OnLMBClicked(InputAction.CallbackContext context) => LMBClicked?.Invoke();
    public void OnRMBClicked(InputAction.CallbackContext context) => RMBClicked?.Invoke();
}
