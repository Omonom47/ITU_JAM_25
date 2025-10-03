using System;
using UnityEngine;
using Grid = Model.Grid;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private TowerPlacement _towerPlacement;
    private void Start()
    {
        var grid = new Grid(5, 5);
        _unitSpawner.SetGrid(grid);
        _towerPlacement.SetGrid(grid);
    }
}