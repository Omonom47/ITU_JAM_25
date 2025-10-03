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
        private int _xSize, _ySize;
        private int _cellSize = 16;
        public Grid(int sizeX, int sizeY)
        {
            _xSize = sizeX;
            _ySize = sizeY;
        }
        
        private readonly List<Cell> _checkPoints = new();
        private readonly List<Cell> _towerPlacements = new();
        public void AddCheckPoint(Cell cell) => _checkPoints.Add(cell);

        public void PlaceTower(Cell cell) => _towerPlacements.Add(cell);
        public Cell[] GetCheckPoints(Team team) =>
            team == Team.Player
                ? _checkPoints.ToArray()
                : _checkPoints?.AsEnumerable().Reverse().ToArray();

        public Cell GetStartingCheckPoint(Team team)
        {
            return team == Team.Player ? _checkPoints[0] : _checkPoints[^1];
        }
    }
}