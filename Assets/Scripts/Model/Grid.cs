using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public enum Team
    {
        Player,
        Enemy
    }

    public class Cell : IEquatable<Cell>
    {
        public int X, Y;

        public bool Equals(Cell other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Cell)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
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
        public bool IsCellOccupiedByTower(Cell cell) => _towerPlacements.Contains(cell);
        public bool IsCellOnCenterWall(Cell cell) => cell.X == 0 || cell.X == -1;
        public bool IsCellOnSide(Cell cell, Team side) => side == Team.Enemy ? cell.X >= 0 : cell.X <= -1;
        public bool IsCellOnPlayerCastle(Cell cell) => cell.X is >= -20 and <= -18 && cell.Y is >= -1 and <= 1;
        public bool IsCellOnEnemyCastle(Cell cell) => cell.X is <=19 and >= 17 && cell.Y is >= -1 and <= 1;

        public Cell GetStartingCheckPoint(Team team)
        {
            return team == Team.Player ? _checkPoints[0] : _checkPoints[^1];
        }
    }
}