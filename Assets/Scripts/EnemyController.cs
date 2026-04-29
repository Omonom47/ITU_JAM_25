using System;
using System.Threading.Tasks;
using Model;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [Serializable]
    public struct EnemyTurn
    {
        public int numUnitsToSpawn;
        public Vector2[] towerPositions;
    }

    [SerializeField] private int _unitPlacementMultiplier = 2;
    [SerializeField] private int _towerPlacementModifier = 2;
    [SerializeField] private float _cursorSpeed = 1f;
    [SerializeField] private GameObject _cursorObject;
    [SerializeField] private Button _unitButton;
    [SerializeField] private Button _towerButton;
    [SerializeField] private Button _readyButton;
    [SerializeField] private int _startAttackingEveryTurn = 6;
    [SerializeField] private int _startPlacingTowersEveryTurn = 9;
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private Shop _shop;
    [SerializeField] private EnemyTurnSequence scriptedTurns;
    public delegate void OnPlacedTower(Vector2 pos);

    public static OnPlacedTower PlacedTower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _turnManager.SetEnemyController(this);
    }

    public void StartEnemyTurn()
    {
        var turnNum = _turnManager.TurnNumber;
        EnemyTurn turn;
        if (scriptedTurns is not null)
        {
            turn = turnNum >= scriptedTurns.turns.Length ? GenerateTurn() : scriptedTurns.turns[turnNum];
        }
        else
        {
            turn = GenerateTurn();
        }
        PerformTurn(turn);
    }

    private EnemyTurn GenerateTurn()
    {
        var ret = new EnemyTurn();
        var turnNum = _turnManager.TurnNumber;
        var shouldAttack = turnNum >= _startAttackingEveryTurn || turnNum % 2 == 1;
        if (shouldAttack)
        {
            ret.numUnitsToSpawn =
                Random.Range(turnNum * _unitPlacementMultiplier,
                    turnNum * turnNum * _unitPlacementMultiplier) + 2;
        }
        else
        {
            ret.numUnitsToSpawn = 0;
        }

        var shouldPlaceTowers = turnNum >= _startPlacingTowersEveryTurn || turnNum % 2 == 0;

        if (shouldPlaceTowers)
        {
            var numTowers = Random.Range(turnNum + _towerPlacementModifier,
                                turnNum + _towerPlacementModifier * 2);
            
            ret.towerPositions = new Vector2[numTowers];
            
            for (int i = 0; i < numTowers; i++)
            {
                Vector2 pos;
                if (turnNum >= 5)
                {
                    var xPos = Random.Range(-10, 12);
                    var yPos = Random.Range(-7, 9);
                    pos = new Vector2(xPos, yPos);
                }
                else
                {
                    var xPos = Random.Range(1, 20);
                    var yPos = Random.Range(-5, 6);
                    pos = new Vector2(xPos, yPos);
                }

                ret.towerPositions[i] = pos;
            }
        }
        

        return ret;
    }

    private async void PerformTurn(EnemyTurn turn)
    {
        for (int i = 0; i < turn.numUnitsToSpawn; i++)
        {
            if (!_shop.CanBuyUnit(Team.Enemy)) break;
            
            var targetPosition = _unitButton.transform.position;
            await MoveCursor(targetPosition);
            
            _unitButton.onClick.Invoke();
            await Awaitable.WaitForSecondsAsync(0.2f);
        }

        if (turn.towerPositions is not null)
        {
            foreach (var t in turn.towerPositions)
            {
                if (!_shop.TryBuyTower(Team.Enemy)) break;

                var targetPosition = _towerButton.transform.position;
                await MoveCursor(targetPosition);


                var pos = t;
                await MoveCursor(pos);
                PlacedTower?.Invoke(pos);

                await Awaitable.WaitForSecondsAsync(1f);
            }
        }

        await Awaitable.WaitForSecondsAsync(1f);
        await MoveCursor(_readyButton.transform.position);
        _readyButton.onClick.Invoke();
    }

    private async Task MoveCursor(Vector3 targetPosition)
    {
        while (Vector3.Distance(_cursorObject.transform.position, targetPosition) > 0.1f)
        {
            _cursorObject.transform.position =
                Vector3.MoveTowards(_cursorObject.transform.position, targetPosition, Time.deltaTime * 150f * _cursorSpeed);
            await Awaitable.NextFrameAsync();
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (scriptedTurns is not null)
        {
            var color = Color.black;
            var counter = 0;
            var lastTurnIndex = scriptedTurns.turns.Length-1;
            foreach (var turn in scriptedTurns.turns)
            {
                foreach (var position in turn.towerPositions)
                {
                    var cellPos = position.ToCell();
                    Gizmos.color = color;
                    Gizmos.DrawCube(cellPos.ToVector2(),Vector3.one);
                }

                counter++;
                color = Color.Lerp(Color.black, Color.white, counter / (float)lastTurnIndex);
            }
        }
    }
#endif
}