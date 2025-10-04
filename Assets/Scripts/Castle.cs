using System;
using Model;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [SerializeField] private Team _team;
    [SerializeField] private int _health = 20;


    private void OnEnable()
    {
        Unit.onFinished += DamageCastle;
    }

    private void DamageCastle(Team team)
    {
        if (team != _team)
        {
            _health--;
        }
    }
    
    
}
