using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    private Vector2 _moveInput;
    private Vector2 _mouseScreenPos;
    private Camera _mainCamera;

    public Vector2 MoveDirection => _moveInput.normalized;
    public Vector2 MouseWorldPosition => _mainCamera.ScreenToWorldPoint(_mouseScreenPos);

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void OnMove(InputValue context)
    {
        _moveInput = context.Get<Vector2>();
    }

    public void OnMouse(InputValue context)
    {
        _mouseScreenPos = context.Get<Vector2>();
    }
}
