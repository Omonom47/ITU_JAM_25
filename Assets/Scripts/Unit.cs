using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Queue<Vector2> _checkPoints;
    private Vector2 _target;
    private bool _isFinished;

    [SerializeField] private float _speed = 10f;

    // Call when spawned
    public void SetCheckPoints(Vector2[] checkPoints)
    {
        if (checkPoints.Length == 0) throw new Exception("No checkpoints provided.");
        
        _checkPoints = new Queue<Vector2>();
        foreach (var checkPoint in checkPoints)
        {
            _checkPoints.Enqueue(checkPoint);
        }

        _target = _checkPoints.Dequeue();
    }

    private void Update()
    {
        if (_isFinished) return;
        
        transform.position =
            Vector2.MoveTowards(transform.position, _target, Time.deltaTime * 5f);
        if (Vector2.Distance(transform.position, _target) < .01f)
        {
            if (_checkPoints.Count == 0)
            {
                _isFinished = true;
            }
            else
            {
                _target = _checkPoints.Dequeue();
            }
        }
    }

    public void TakeDamage(int damage)
    {
        Destroy(this.gameObject);
    }
}