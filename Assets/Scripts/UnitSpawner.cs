using System;
using System.Linq;
using Model;
using UnityEngine;
using Grid = Model.Grid;

public class UnitSpawner : MonoBehaviour
{
    private Grid _grid;
    
    [SerializeField] private Unit _unit;

    public void SetGrid(Grid grid)
    {
        _grid = grid;
    }

    public void SpawnPlayerUnit() => SpawnUnit(Team.Player);
    public void SpawnEnemyUnit() => SpawnUnit(Team.Enemy);
    
    private void SpawnUnit(Team team)
    {
        var unit = 
            Instantiate(_unit, _grid.GetStartingCheckPoint(team).ToVector2(), Quaternion.identity);
        unit.SetCheckPoints(_grid.GetCheckPoints(team).Select(cell => cell.ToVector2()).ToArray());
        unit.SetTeam(team);
    }
}