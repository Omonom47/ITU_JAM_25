using System;
using System.Linq;
using Model;
using ScriptableObjects;
using UnityEngine;
using Grid = Model.Grid;

public class UnitSpawner : MonoBehaviour
{
    private Grid _grid;

    public delegate void OnUnitSpawned(Unit spawned, Team team);
    public static OnUnitSpawned onUnitSpawned;

    [SerializeField] private Shop _shop;
    
    [SerializeField] private Unit _unit;

    [SerializeField] private Sprite _enemySprite;

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
        var unit = 
            Instantiate(_unit, _grid.GetStartingCheckPoint(team).ToVector2(), Quaternion.identity);
        unit.SetCheckPoints(_grid.GetCheckPoints(team).Select(cell => cell.ToVector2()).ToArray());
        unit.Team = team;
        if (unit.Team == Team.Enemy)
        {
            unit.gameObject.GetComponent<SpriteRenderer>().sprite = _enemySprite;
        }
        unit.gameObject.SetActive(false);
        onUnitSpawned?.Invoke(unit, team);
    }
}