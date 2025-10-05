using System;
using Model;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [SerializeField] private Team _team;
    [SerializeField] private IntVariable _health;

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

    private void DamageCastle(Unit attacker)
    {
        if (attacker.Team != _team)
        {
            _health.Value--;
        }

        if (_health.Value <= 0)
        {
            onGameOver?.Invoke();
        }
    }
}