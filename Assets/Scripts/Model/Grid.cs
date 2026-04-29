using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public enum Team
    {
        Player,
        Enemy
    }

    public class Cell
    {
        public int X, Y;
    }

    public class Grid
    {
        private readonly List<Cell> _checkPoints = new();
        private readonly List<Cell> _towerPlacements = new();
        public void AddCheckPoint(Cell cell) => _checkPoints.Add(cell);

        public void PlaceTower(Cell cell) => _towerPlacements.Add(cell);
        public Cell[] GetCheckPoints(Team team) =>
            team == Team.Player
                ? _checkPoints.ToArray()
                : _checkPoints?.AsEnumerable().Reverse().ToArray();

        public bool IsCellOnPath(Cell cell)
        {
            for (int i = 0; i < _checkPoints.Count-1; i++)
            {
                var cur = _checkPoints[i];
                var next = _checkPoints[i + 1];
                if (cell.X >= cur.X && cell.X <= next.X && cell.Y == cur.Y && cell.Y == next.Y)
                {
                    return true;
                }if (cell.Y >= cur.Y && cell.Y <= next.Y && cell.X == cur.X && cell.X == next.X)
                {
                    return true;
                }
            }
            return false;
        }
        
        public Cell GetStartingCheckPoint(Team team)
        {
            return team == Team.Player ? _checkPoints[0] : _checkPoints[^1];
        }
    }
}