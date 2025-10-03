using System;
using UnityEngine;

public class TowerShooting: MonoBehaviour
{
    [SerializeField] private float _attackRange = 5;
    [SerializeField] private LayerMask _whatToShoot;
    [SerializeField] private float _shotCooldown = 0.5f;

    private Unit _currentTarget;
    private float _cooldownLeft;

    private void Update()
    {
        _cooldownLeft -= Time.deltaTime;
        
        if (_currentTarget is null)
        {
            Vector3 selfPosition = transform.position;
            var units = Physics2D.OverlapCircleAll(selfPosition, _attackRange,_whatToShoot);
            var minDist = -1.0f;
            Collider2D cur = null;
            foreach (var unit in units)
            {
                var dist = (unit.transform.position - selfPosition).sqrMagnitude;
                if (dist <= minDist || cur is null)
                {
                    minDist = dist;
                    cur = unit;
                }
            }
            _currentTarget =  cur?.gameObject.GetComponent<Unit>();
        }
        
        if (_currentTarget is not null && _cooldownLeft <= 0)
        {
            _currentTarget.TakeDamage(1);
            _currentTarget = null;
            _cooldownLeft = _shotCooldown;
        }
    }

}
