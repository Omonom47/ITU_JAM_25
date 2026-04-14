using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Turn Sequence")]
    public class EnemyTurnSequence:ScriptableObject
    {
        public EnemyController.EnemyTurn[] turns;
    }
}