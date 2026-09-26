using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Input Reader", fileName = "InputReader")]
public class InputReader : ScriptableObject
{
    //Events based on which inputs are being used
    public event UnityAction<Vector2> OnMoveEvent;
    public event UnityAction OnMenuOpenEvent;
    public event UnityAction OnJumpEvent;
    public InputActionAsset _asset;
    private InputAction _move, _menu, _jump;
    void OnEnable()
    {
        _move = _asset.FindAction("MoveInput");
        _jump = _asset.FindAction("Jump");
        _menu = _asset.FindAction("OpenMenu");

        _move.performed += OnMove;
        _jump.performed += OnJump;
        _menu.started += OnOpenMenu;
        
        _move.Enable();
        _jump.Enable();
        _menu.Enable();
    }

    void OnDisable()
    {
        _move.performed -= OnMove;
        _jump.performed -= OnJump;
        _menu.started -= OnOpenMenu;
        
        _move.Disable();
        _jump.Disable();
        _menu.Disable();
    }
    public void DisablePlayerControls()
    {
        _move.Disable();
        _jump.Disable();
    }
    public void EnablePlayerControls()
    {
        _move.Enable();
        _jump.Enable();
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }
    private void OnOpenMenu(InputAction.CallbackContext context)
    {
        OnMenuOpenEvent?.Invoke();
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        OnJumpEvent?.Invoke();
    }
}
