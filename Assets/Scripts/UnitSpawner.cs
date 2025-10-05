using System;
using System.Linq;
using Model;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using Grid = Model.Grid;

public class UnitSpawner : MonoBehaviour
{
    private Grid _grid;

    public delegate void OnUnitSpawned(Unit spawned, Team team);
    public static OnUnitSpawned onUnitSpawned;

    [SerializeField] private Shop _shop;
    
    [FormerlySerializedAs("_unit")] [SerializeField] private Unit _playerUnit;
    [SerializeField] private Unit _enemyUnit;

    public void SetGrid(Grid grid)
    {
        _grid = grid;
    }

    public void BuyPlayerUnit() => BuyUnit(Team.Player);

    public void BuyEnemyUnit() => BuyUnit(Team.Enemy);

    private void BuyUnit(Team team)
    {
        if (_shop.TryBuyUnit(team))
        {
            SpawnUnit(team);
        }
    }
    
    public void SpawnPlayerUnit() => SpawnUnit(Team.Player);
    public void SpawnEnemyUnit() => SpawnUnit(Team.Enemy);
    
    private void SpawnUnit(Team team)
    {
        Unit unit;
            
        if (team == Team.Enemy)
        {
            unit = Instantiate(_enemyUnit, _grid.GetStartingCheckPoint(team).ToVector2(), Quaternion.identity);
            unit.SetCheckPoints(_grid.GetCheckPoints(team).Select(cell => cell.ToVector2()).ToArray());
            unit.Team = team;
        }
        else
        {
            unit = Instantiate(_playerUnit, _grid.GetStartingCheckPoint(team).ToVector2(), Quaternion.identity);
            unit.SetCheckPoints(_grid.GetCheckPoints(team).Select(cell => cell.ToVector2()).ToArray());
            unit.Team = team;
        }
        unit.gameObject.SetActive(false);
        onUnitSpawned?.Invoke(unit, team);
    }
}