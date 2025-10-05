using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Model;
using ScriptableObjects;

public class TowerPlacement : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] private TowerShooting towerPrefab;
    [SerializeField] private Sprite _enemyTowerSprite;
    [SerializeField] private Shop _shop;
    
    private InputSystem_Actions _inputSystemActions;
    private Model.Grid _grid;

    private Vector2 _mousePosition;
    private bool _canPlace;

    private void OnEnable()
    {
        _inputSystemActions = new InputSystem_Actions();
        _inputSystemActions.Player.SetCallbacks(this);
        _inputSystemActions.Player.Enable();
        EnemyController.placedTower += EnemyPlaceTower;
    }
    
    private void OnDisable()
    {
        _inputSystemActions.Player.Disable();
        EnemyController.placedTower -= EnemyPlaceTower;
    }

    public void OnLook(InputAction.CallbackContext context) => 
        _mousePosition = context.action.ReadValue<Vector2>();

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame() && _canPlace)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(_mousePosition);
            var cell = mouseWorldPos.ToCell();
            
            if (Mathf.Abs(cell.X) > 19) return;
            if (!_shop.TryBuyTower(Team.Player)) return;
    
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
    
    public void EnemyPlaceTower(Vector2 pos)
    {
        var cell = pos.ToCell();
        _grid.PlaceTower(cell);
            
        var tower = Instantiate(towerPrefab, cell.ToVector2(), Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sprite = _enemyTowerSprite;
        tower.SetTeam(Team.Enemy);
    }
}