using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Model;

public class TowerPlacement : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] private TowerShooting towerPrefab;
    
    private InputSystem_Actions _inputSystemActions;
    private Model.Grid _grid;

    private Vector2 _mousePosition;
    private bool _canPlace;

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

    public void OnLook(InputAction.CallbackContext context) => 
        _mousePosition = context.action.ReadValue<Vector2>();

    public void OnAttack(InputAction.CallbackContext context)
    {
        
        if (context.action.WasPressedThisFrame() && _canPlace)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(_mousePosition);
            var cell = mouseWorldPos.ToCell();
            _grid.PlaceTower(cell);
            
            var tower = Instantiate(towerPrefab, cell.ToVector2(), Quaternion.identity);
            tower.SetTeam(Team.Player);
            _canPlace = false;
        }
    }

    public void SetGrid(Model.Grid grid)
    {
        _grid = grid;
    }

    public void EnableTowerPlacement()
    {
        _canPlace = true;
    }
}