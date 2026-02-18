using Model;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Shop")]
    public class Shop : ScriptableObject
    {
        [SerializeField] private int _unitCost;
        [SerializeField] private int _towerCost;

        [SerializeField] private IntVariable _playerMoney;
        [SerializeField] private IntVariable _enemyMoney;

        public bool CanBuyUnit(Team team)
        {
            var money = team == Team.Player ? _playerMoney : _enemyMoney;
            return money.Value >= _unitCost;
        }
        
        public bool TryBuyUnit(Team team)
        {
            var money = team == Team.Player ? _playerMoney : _enemyMoney;
            if (money.Value >= _unitCost)
            {
                money.Value -= _unitCost;
                return true;
            }

            return false;
        }

        public bool TryBuyTower(Team team)
        {
            var money = team == Team.Player ? _playerMoney : _enemyMoney;
            if (money.Value >= _towerCost)
            {
                money.Value -= _towerCost;
                return true;
            }

            return false;
        }
    }
}