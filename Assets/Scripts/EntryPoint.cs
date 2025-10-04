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