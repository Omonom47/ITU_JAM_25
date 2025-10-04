using System;
using Model;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [SerializeField] private Team _team;
    [SerializeField] private int _health = 20;

    public delegate void OnGameOver();

    public static OnGameOver onGameOver;

    private void OnEnable()
    {
        Unit.onFinished += DamageCastle;
    }
    
    private void OnDisable()
    {
        Unit.onFinished -= DamageCastle;
    }

    private void DamageCastle(Team team)
    {
        if (team != _team)
        {
            _health--;
        }

        if (_health<= 0)
        {
            onGameOver?.Invoke();
        }
    }
    
    
}
