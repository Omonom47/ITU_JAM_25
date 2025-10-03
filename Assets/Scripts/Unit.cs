using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Transform[] _checkPoints;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Transform _start;
    
    private Transform _target;
    private int _targetIndex;

    private void Start()
    {
        _target = _checkPoints[0];
        _targetIndex = 0;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * _speed);
        if (Vector3.Distance(_target.position, transform.position) < 1f)
        {
            _targetIndex++;
            _target = _checkPoints[_targetIndex];
        }
    }
}
