using System;
using System.Collections.Generic;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    private Queue<Vector2> _checkPoints;
    private Vector2 _target;
    private bool _isFinished;
    private Animator _anim;
    public Team Team { get; set; }

    [SerializeField] private float _speed = 10f;
    [SerializeField] private int _health = 1;
    [SerializeField] private AudioClip _deathSound;
    
    public delegate void OnFinished(Unit finished);
    public static OnFinished onFinished;

    public delegate void OnDeath(Unit dying);
    public static OnDeath onDeath;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

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
                onFinished?.Invoke(this);
            }
            else
            {
                _target = _checkPoints.Dequeue();
                while (Random.value < 0.2f && _checkPoints.Count > 0)
                {
                    _target = _checkPoints.Dequeue();
                }
            }
        }
    }
    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {

            var pitch = Random.Range(0.9f, 1.1f);
            PlayClipAt(_deathSound, transform.position, pitch, 1);
            onDeath?.Invoke(this);
        }
    }

    private AudioSource PlayClipAt(AudioClip clip, Vector3 position, float pitch, float volume)
    {
        var tempGo = new GameObject("tempAudio")
        {
            transform =
            {
                position = position
            }
        };
        var aSource = tempGo.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.pitch = pitch;
        aSource.volume = volume;
        aSource.Play();
        Destroy(tempGo,clip.length);
        return aSource;
    }
}