using System;
using Model;
using UnityEngine;
using Grid = Model.Grid;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private TowerPlacement _towerPlacement;
    private void Start()
    {
        var grid = new Grid(5, 5);
        grid.AddCheckPoint(new Cell{X = 0, Y = 0});
        grid.AddCheckPoint(new Cell{X = 1, Y = 0});
        grid.AddCheckPoint(new Cell{X = 1, Y = 1});
        grid.AddCheckPoint(new Cell{X = 0, Y = 1});
        _unitSpawner.SetGrid(grid);
        _towerPlacement.SetGrid(grid);
    }
}