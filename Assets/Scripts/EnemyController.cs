using System.Collections;
using Model;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    struct EnemyTurn
    {
        public int NumUnitsToSpawn, NumTowersToPlace;
    }

    [SerializeField] private int _unitPlacementMultiplier = 2;
    [SerializeField] private int _towerPlacementModifier = 2;
    [SerializeField] private float _cursorSpeed = 1f;
    [SerializeField] private GameObject _cursorObject;
    [SerializeField] private Button _unitButton;
    [SerializeField] private Button _readyButton;
    [SerializeField] private int _startAttackingEveryTurn = 6;
    [SerializeField] private int _startPlacingTowersEveryTurn = 9;
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private Shop _shop;

    public delegate void OnPlacedTower(Vector2 pos);

    public static OnPlacedTower placedTower;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _turnManager.SetEnemyController(this);
    }

    public void StartEnemyTurn()
    {
        var turn = GenerateTurn();
        StartCoroutine(PerformTurn(turn));
    }

    private EnemyTurn GenerateTurn()
    {
        var ret = new EnemyTurn();
        var turnNum = _turnManager.TurnNumber;
        var shouldAttack = turnNum >= _startAttackingEveryTurn || turnNum % 2 == 1;
        if (shouldAttack)
        {
            ret.NumUnitsToSpawn =
                Random.Range(turnNum * _unitPlacementMultiplier,
                    turnNum * turnNum * _unitPlacementMultiplier) + 2;
        }
        else
        {
            ret.NumUnitsToSpawn = 0;
        }

        var shouldPlaceTowers = turnNum >= _startPlacingTowersEveryTurn || turnNum % 2 == 0;

        if (shouldPlaceTowers)
        {
            ret.NumTowersToPlace = Random.Range(turnNum + _towerPlacementModifier,
                turnNum + _towerPlacementModifier * 2);
        }
        else
        {
            ret.NumTowersToPlace = 0;
        }

        return ret;
    }

    private IEnumerator PerformTurn(EnemyTurn turn)
    {
        for (int i = 0; i < turn.NumUnitsToSpawn; i++)
        {
            _unitButton.onClick.Invoke();
            yield return new WaitForSeconds(0.2f);
        }

        for (int i = 0; i < turn.NumTowersToPlace; i++)
        {
            if (!_shop.TryBuyTower(Team.Enemy)) break;

            if (_turnManager.TurnNumber >= 5)
            {
                var xPos = Random.Range(-5, 20);
                var yPos = Random.Range(-7, 9);
                Vector2 pos = new Vector2(xPos, yPos);
                placedTower?.Invoke(pos);
            }
            else
            {
                var xPos = Random.Range(1, 20);
                var yPos = Random.Range(-5, 6);
                Vector2 pos = new Vector2(xPos, yPos);
                placedTower?.Invoke(pos);
            }

            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);
        _readyButton.onClick.Invoke();
    }
}