using System;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;

public class TowerShooting : MonoBehaviour
{
    [SerializeField] private float _attackRange = 5;
    [SerializeField] private float _shotCooldown = 0.5f;
    [SerializeField] private GameObject _projectilePrefab;   
    [SerializeField] private float _projectileSpeed = 10f;   
    [SerializeField] private Transform _firePoint;           

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
            ShootProjectile(_currentTarget);
            _currentTarget = null;
            _cooldownLeft = _shotCooldown;

            _audio.pitch = Random.Range(0.95f, 1.1f);
            _audio.PlayOneShot(_audio.clip);
        }
    }

    private void ShootProjectile(Unit target)
    {
        if (_projectilePrefab == null || target == null) return;

        Vector3 spawnPos = _firePoint ? _firePoint.position : transform.position;
        GameObject proj = Instantiate(_projectilePrefab, spawnPos, Quaternion.identity);
        
        StartCoroutine(MoveProjectile(proj, target));
    }

    private System.Collections.IEnumerator MoveProjectile(GameObject projectile, Unit target)
    {
        while (projectile != null && target != null)
        {
            projectile.transform.position = Vector3.MoveTowards(
                projectile.transform.position,
                target.transform.position,
                _projectileSpeed * Time.deltaTime);

            if (Vector3.Distance(projectile.transform.position, target.transform.position) < 0.1f)
            {
                target.TakeDamage(1);
                Destroy(projectile);
                yield break;
            }

            yield return null;
        }

        if (projectile != null)
            Destroy(projectile);
    }

    public void SetTeam(Team team) => _team = team;
}
