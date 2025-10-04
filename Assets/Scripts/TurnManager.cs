using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum TurnPhase
    {
        Prep,Simulation
    }
    [SerializeField] private float _timeBetweenUnits = 0.5f;
    
    private bool _playerReady = false;
    private bool _enemyReady = false;
    
    private static readonly List<Unit> _playerUnits = new();
    private static readonly List<Unit> _enemyUnits = new();

    private readonly Queue<Unit> _playerWave = new();
    private readonly Queue<Unit> _enemyWave = new();

    private int _turnNumber = 0;
    public TurnPhase Phase { get; private set; }
    private void OnEnable()
    {
        UnitSpawner.onUnitSpawned += RegisterUnit;
        Unit.onDeath += DeregisterUnit;
        Unit.onFinished += DeregisterUnit;
    }

    private void OnDisable()
    {
        UnitSpawner.onUnitSpawned -= RegisterUnit;
        Unit.onDeath -= DeregisterUnit;
        Unit.onFinished -= DeregisterUnit;
    }
    
    public void RegisterPlayerReady()
    {
        _playerReady = true;
        CheckIfBothReady();
    }

    public void RegisterEnemyReady()
    {
        _enemyReady = true;
        CheckIfBothReady();
    }

    private IEnumerator ReleaseUnits()
    {
        if (_enemyWave.TryDequeue(out var eUnit))
        {
            eUnit.gameObject.SetActive(true);
        }


        if (_playerWave.TryDequeue(out var pUnit))
        {
            pUnit.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(_timeBetweenUnits);
        StartCoroutine(ReleaseUnits());
    } 
    private void CheckIfBothReady()
    {
        if (_playerReady && _enemyReady)
        {
            _playerReady = _enemyReady = false;
            StartTurn();
        }
    }

    private void StartTurn()
    {
        StartCoroutine(ReleaseUnits());
        Phase = TurnPhase.Simulation;
    }
    
    private void NextTurn()
    {
        StopAllCoroutines();
        _turnNumber++;
        Phase = TurnPhase.Prep;
    }
    
    private void RegisterUnit(Unit toRegister, Team team)
    {
        if (team == Team.Player)
        {
            _playerUnits.Add(toRegister);
            _playerWave.Enqueue(toRegister);
        }
        else
        {
            _enemyUnits.Add(toRegister);
            _enemyWave.Enqueue(toRegister);
        }
    }

    private void DeregisterUnit(Unit toDeregister)
    {
        if (toDeregister.Team == Team.Player)
            _playerUnits.Remove(toDeregister);
        else
            _enemyUnits.Remove(toDeregister);
        Destroy(toDeregister.gameObject);
        if (_playerUnits.Count == 0 && _enemyUnits.Count == 0)
        {
            NextTurn();
        }
    }
    
    public static List<Unit> GetOpposingUnits(Team team)
    {
        if (team == Team.Player)
            return _enemyUnits;
        else
            return _playerUnits;
        
    }
    
    
}
