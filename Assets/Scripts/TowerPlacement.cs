using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TowerPlacement : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] private GameObject _prefab;
    private InputSystem_Actions _inputSystemActions;

    private Vector2 _position;

    private void OnEnable()
    {
        _inputSystemActions = new InputSystem_Actions();
        _inputSystemActions.Player.SetCallbacks(this);
        _inputSystemActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputSystemActions.Player.Disable();
    }   

    public void OnMove(InputAction.CallbackContext context)
    {
    }

    public void OnLook(InputAction.CallbackContext context) => 
        _position = context.action.ReadValue<Vector2>();

    public void OnAttack(InputAction.CallbackContext context)
    {
        Instantiate(_prefab, _position, Quaternion.identity);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
    }

    public void OnJump(InputAction.CallbackContext context)
    {
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
    }

    public void OnNext(InputAction.CallbackContext context)
    {
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
    }
}