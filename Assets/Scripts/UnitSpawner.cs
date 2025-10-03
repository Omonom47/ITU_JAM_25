using System;
using System.Linq;
using Model;
using UnityEngine;
using UnityEngine.InputSystem;
using Grid = Model.Grid;

public class UnitSpawner : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    private Grid _grid;
    private InputSystem_Actions _inputSystemActions;
    [SerializeField] private Unit _unit;

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

    public void SetGrid(Grid grid)
    {
        _grid = grid;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
    }

    public void SpawnPlayerUnit() => SpawnUnit(Team.Player);
    public void SpawnEnemyUnit() => SpawnUnit(Team.Enemy);
    
    private void SpawnUnit(Team team)
    {
        var unit = 
            Instantiate(_unit, _grid.GetStartingCheckPoint(team).ToVector2(), Quaternion.identity);
        unit.SetCheckPoints(_grid.GetCheckPoints(team).Select(cell => cell.ToVector2()).ToArray());
    }
}