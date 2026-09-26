using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

[CreateAssetMenu(menuName = "Input Reader", fileName = "InputReader")]
public class InputReader : ScriptableObject
{
    //Events based on which inputs are being used
    public event UnityAction<Vector2> OnMoveEvent;
    public event UnityAction OnUsePrimaryWeaponEvent;
    public event UnityAction OnUseSecondaryWeaponEvent;
    public event UnityAction OnDodgeEvent;
    public event UnityAction OnInteractEvent;
    public event UnityAction OnMenuOpenEvent;
    public event UnityAction OnOpenInventoryEvent;
    public event UnityAction OnOpenMapEvent;
    
    public InputActionAsset _asset;
    private InputAction _move, _primary, _secondary, _dodge, _interact, _menu, _inventory, _map;
    void OnEnable()
    {
        _move = _asset.FindAction("MoveInput");
        _primary = _asset.FindAction("PrimaryAttack");
        _secondary = _asset.FindAction("SecondaryAttack");
        _dodge = _asset.FindAction("Dodge");
        _interact = _asset.FindAction("Interact");
        _menu = _asset.FindAction("OpenOptionsMenu");
        _inventory = _asset.FindAction("OpenInventory");
        _map = _asset.FindAction("OpenMap");

        _move.performed += OnMove;
        _primary.performed += OnPrimaryUsed;
        _secondary.performed += OnSecondaryUsed;
        _dodge.performed += OnDodge;
        _interact.performed += OnInteract;
        _menu.started += OnOpenMenu;
        _inventory.performed += OnOpenInventory;
        _map.performed += OnOpenMap;
        
        _move.Enable();
        _primary.Enable();
        _secondary.Enable();
        _dodge.Enable();
        _interact.Enable();
        _menu.Enable();
        _inventory.Enable();
        _map.Enable();
    }

    void OnDisable()
    {
        _move.performed -= OnMove;
        _primary.performed -= OnPrimaryUsed;
        _secondary.performed -= OnSecondaryUsed;
        _dodge.performed -= OnDodge;
        _interact.performed -= OnInteract;
        _menu.started -= OnOpenMenu;
        _inventory.performed -= OnOpenInventory;
        _map.performed -= OnOpenMap;
        
        _move.Disable();
        _primary.Disable();
        _secondary.Disable();
        _dodge.Disable();
        _interact.Disable();
        _menu.Disable();
        _inventory.Disable();
        _map.Disable();
    }



    public void DisablePlayerControls()
    {
        _move.Disable();
        _primary.Disable();
        _secondary.Disable();
        _dodge.Disable();
        _interact.Disable();
    }
    public void EnablePlayerControls()
    {
        _move.Enable();
        _primary.Enable();
        _secondary.Enable();
        _dodge.Enable();
        _interact.Enable();
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }
    private void OnPrimaryUsed(InputAction.CallbackContext context)
    {
        if(context.performed) OnUsePrimaryWeaponEvent?.Invoke();
    }
    private void OnSecondaryUsed(InputAction.CallbackContext context)
    {
        if(context.performed) OnUseSecondaryWeaponEvent?.Invoke();
    }
    private void OnDodge(InputAction.CallbackContext context)
    {
        if(context.performed) OnDodgeEvent?.Invoke();
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed) OnInteractEvent?.Invoke();
    }
    private void OnOpenMenu(InputAction.CallbackContext context)
    {
        if(context.performed) OnMenuOpenEvent?.Invoke();
    }
    private void OnOpenInventory(InputAction.CallbackContext context)
    {
        if(context.performed) OnOpenInventoryEvent?.Invoke();
    }
    private void OnOpenMap(InputAction.CallbackContext context)
    {
        if(context.performed) OnOpenMapEvent?.Invoke();
    }
}
