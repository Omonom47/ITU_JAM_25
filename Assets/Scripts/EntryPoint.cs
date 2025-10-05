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
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private Transform[] _checkPoints;

    [SerializeField] private int _startingMoney = 100;
    
    [SerializeField] private IntVariable _playerMoney;
    [SerializeField] private IntVariable _enemyMoney;
    
    


    private void Awake()
    {
        var grid = new Grid();
        foreach (var checkPoint in _checkPoints)
        {
            Vector2 position = checkPoint.position;
            grid.AddCheckPoint(position.ToCell());
        }
        _unitSpawner.SetGrid(grid);
        _towerPlacement.SetGrid(grid);
        _playerMoney.Value = _startingMoney;
        _enemyMoney.Value = _startingMoney;
    }

    private void Start()
    {
        _turnManager.FirstTurn();
    }

    private void OnEnable()
    {
        Castle.onGameOver += RestartGame;
    }

    private void OnDisable()
    {
        Castle.onGameOver -= RestartGame;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    
}