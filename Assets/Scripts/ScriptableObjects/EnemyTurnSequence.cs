using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Turn Sequence")]
    public class EnemyTurnSequence:ScriptableObject
    {
        public EnemyController.EnemyTurn[] turns;

        private void OnValidate()
        {
            foreach (var turn in turns)
            {
                for (var i = 0; i < turn.towerPositions.Length; i++)
                {
                    var pos = turn.towerPositions[i];
                    if (pos.x >= 20)
                    {
                        pos.x = 19.5f;
                    }else if (pos.x <= -20)
                    {
                        pos.x = -19.5f;
                    }

                    if (pos.y >= 10)
                    {
                        pos.y = 9.5f;
                    }else if (pos.y <= -9)
                    {
                        pos.y = -8.5f;
                    }

                    turn.towerPositions[i] = pos;
                }
            }
        }
    }
}