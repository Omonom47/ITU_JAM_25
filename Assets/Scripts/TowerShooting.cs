using System;
using Model;
using UnityEngine;

public class TowerShooting: MonoBehaviour
{
    [SerializeField] private float _attackRange = 5;
    [SerializeField] private float _shotCooldown = 0.5f;

    private Unit _currentTarget;
    private float _cooldownLeft;
    private Team _team;
    private float _atkRangeSqr;
    private AudioSource _audio;

    private void Start()
    {
        _atkRangeSqr = _attackRange * _attackRange;
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _cooldownLeft -= Time.deltaTime;
        
        if (_currentTarget is null)
        {
            Vector3 selfPosition = transform.position;

            var units = TurnManager.GetOpposingUnits(_team);
            
            foreach (var unit in units)
            {
                var dist = (selfPosition - unit.transform.position).sqrMagnitude;
                if (dist <= _atkRangeSqr)
                {
                    _currentTarget = unit;
                    break;
                }
            }
        }
        
        if (_currentTarget is not null && _cooldownLeft <= 0)
        {
            _currentTarget.TakeDamage(1);
            _currentTarget = null;
            _cooldownLeft = _shotCooldown;
            _audio.PlayOneShot(_audio.clip);
        }
    }

    public void SetTeam(Team team) => _team = team;

}
