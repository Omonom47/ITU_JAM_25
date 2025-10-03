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
        public Grid(int sizeX, int sizeY)
        {
            
        }
        
        private readonly List<Cell> _checkPoints = new();
        public void AddCheckPoint(Cell cell) => _checkPoints.Add(cell);
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