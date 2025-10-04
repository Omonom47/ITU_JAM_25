using System;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using Grid = Model.Grid;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private TowerPlacement _towerPlacement;
    [SerializeField] private Transform[] _checkPoints;

    private static readonly List<Unit> _playerUnits = new();
    private static readonly List<Unit> _enemyUnits = new();

    private void Start()
    {
        var grid = new Grid();
        foreach (var checkPoint in _checkPoints)
        {
            Vector2 position = checkPoint.position;
            grid.AddCheckPoint(position.ToCell());
        }
        _unitSpawner.SetGrid(grid);
        _towerPlacement.SetGrid(grid);
    }

    private void OnEnable()
    {
        Castle.onGameOver += RestartGame;
        UnitSpawner.onUnitSpawned += RegisterUnit;
        Unit.onDeath += DeregisterUnit;
    }

    private void OnDisable()
    {
        Castle.onGameOver -= RestartGame;
        UnitSpawner.onUnitSpawned -= RegisterUnit;
        Unit.onDeath -= DeregisterUnit;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void RegisterUnit(Unit toRegister, Team team)
    {
        if (team == Team.Player)
            _playerUnits.Add(toRegister);
        else
            _enemyUnits.Add(toRegister);
    }

    private void DeregisterUnit(Unit toDeregister)
    {
        if (toDeregister.Team == Team.Player)
            _playerUnits.Remove(toDeregister);
        else
            _enemyUnits.Remove(toDeregister);
        Destroy(toDeregister.gameObject);
    }

    public static List<Unit> GetOpposingUnits(Team team)
    {
        if (team == Team.Player)
            return _enemyUnits;
        else
            return _playerUnits;
        

    }
}