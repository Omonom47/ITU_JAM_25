using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum TurnPhase
    {
        Prep,
        Simulation
    }

    [SerializeField] private float _timeBetweenUnits = 0.5f;
    [SerializeField] private IntVariable _playerMoney;
    [SerializeField] private IntVariable _enemyMoney;
    [SerializeField] private IntVariable _playerUnitsQueuedUp;
    [SerializeField] private IntVariable _enemyUnitsQueuedUp;
    [SerializeField] private AudioClip _fightMusic;
    [SerializeField] private AudioClip _prepMusic;
    
    private bool _playerReady = false;
    private bool _enemyReady = false;
    private EnemyController _enemyController;
    private AudioSource _audio;

    private static readonly List<Unit> _playerUnits = new();
    private static readonly List<Unit> _enemyUnits = new();

    private readonly Queue<Unit> _playerWave = new();
    private readonly Queue<Unit> _enemyWave = new();

    public int TurnNumber { get; private set; }
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

    private void Start()
    {
        RunTurnTimer();
        _audio = GetComponent<AudioSource>();
    }

    private async void RunTurnTimer()
    {
        while (true)
        {
            await Awaitable.WaitForSecondsAsync(10f);
            if (_playerUnits.Count == 0 && _enemyUnits.Count == 0 &&
                _playerUnitsQueuedUp.Value == 0 && _enemyUnitsQueuedUp.Value == 0 &&
                Phase == TurnPhase.Simulation)
            {
                NextTurn();
            }
        }
    }

    public void RegisterPlayerReady()
    {
        if (Phase == TurnPhase.Prep)
        {
            _playerReady = true;
            CheckIfBothReady();
        }
    }

    public void RegisterEnemyReady()
    {
        if (Phase == TurnPhase.Prep)
        {
            _enemyReady = true;
            CheckIfBothReady();
        }
    }

    private IEnumerator ReleaseUnits()
    {
        var waitTime = Random.Range(0.05f, _timeBetweenUnits);
        if (_enemyWave.TryDequeue(out var eUnit))
        {
            eUnit.gameObject.SetActive(true);
            _enemyUnitsQueuedUp.Value--;
        }


        if (_playerWave.TryDequeue(out var pUnit))
        {
            pUnit.gameObject.SetActive(true);
            _playerUnitsQueuedUp.Value--;
        }

        yield return new WaitForSeconds(waitTime);
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

    public void FirstTurn()
    {
        Phase = TurnPhase.Prep;
        _enemyController.StartEnemyTurn();
    }

    private void StartTurn()
    {
        StartCoroutine(ReleaseUnits());
        Phase = TurnPhase.Simulation;
        _audio.Stop();
        _audio.clip = _fightMusic;
        _audio.volume = 1.0f;
        _audio.Play();
    }

    private void NextTurn()
    {
        StopAllCoroutines();
        _audio.Stop();
        _audio.clip = _prepMusic;
        _audio.volume = 0.5f;
        _audio.Play();
        TurnNumber++;
        Phase = TurnPhase.Prep;
        _playerMoney.Value += 100;
        _enemyMoney.Value += 100;
        _enemyController.StartEnemyTurn();
    }

    private void RegisterUnit(Unit toRegister, Team team)
    {
        if (team == Team.Player)
        {
            _playerUnits.Add(toRegister);
            _playerWave.Enqueue(toRegister);
            _playerUnitsQueuedUp.Value++;
        }
        else
        {
            _enemyUnits.Add(toRegister);
            _enemyWave.Enqueue(toRegister);
            _enemyUnitsQueuedUp.Value++;
        }
    }

    private void DeregisterUnit(Unit toDeregister)
    {
        if (toDeregister.Team == Team.Player)
        {
            _playerUnits.Remove(toDeregister);
        }
        else
        {
            _enemyUnits.Remove(toDeregister);
        }
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

    public void SetEnemyController(EnemyController ec)
    {
        _enemyController = ec;
    }
}