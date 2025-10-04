using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    struct EnemyTurn
    {
        public int NumUnitsToSpawn , NumTowersToPlace;
        
    }

    [SerializeField] private int _unitPlacementMultiplier = 2;
    [SerializeField] private int _towerPlacementModifier = 2;
    [SerializeField] private float _cursorSpeed = 1f;
    [SerializeField] private GameObject _cursorObject;
    [SerializeField] private Button _unitButton;
    [SerializeField] private Button _readyButton;
    private TurnManager _turnManager;

    public delegate void OnPlacedTower(Vector2 pos);
    public static OnPlacedTower placedTower;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _turnManager = GameObject.FindWithTag("EntryPoint").GetComponent<TurnManager>();
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
        ret.NumUnitsToSpawn = 
            Random.Range(turnNum * _unitPlacementMultiplier,
                turnNum * turnNum * _unitPlacementMultiplier);

        ret.NumTowersToPlace = Random.Range(turnNum + _towerPlacementModifier,
            turnNum + _towerPlacementModifier * 2);

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
