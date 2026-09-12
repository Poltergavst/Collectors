using UnityEngine;
using UnityEngine.InputSystem;

public class CursorInteractionHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerInputReader _inputReader;

    private BaseBuilder _activeBuilder;

    private void OnEnable()
    {
        _inputReader.LMBClicked += OnLeftClick;
        _inputReader.RMBClicked += OnRightClick;
    }

    private void OnDisable()
    {
        _inputReader.LMBClicked -= OnLeftClick;
        _inputReader.RMBClicked -= OnRightClick;
    }

    private void OnLeftClick()
    {
        if (_activeBuilder != null && _activeBuilder.IsBuildModeActive)
        {
            _activeBuilder.HandlePlacementClick();
            return;
        }

        _activeBuilder = null;

        Vector2 mousePositon = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mousePositon);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
        {
            if (hitInfo.collider.TryGetComponent(out ICursorInteractable interactable))
            {
                interactable.Interact();

                if (interactable is BaseBuilder builder)
                    _activeBuilder = builder;
            }
        }
    }

    private void OnRightClick()
    {
        _activeBuilder?.ExitBuildMode();
        _activeBuilder = null;
    }
}
