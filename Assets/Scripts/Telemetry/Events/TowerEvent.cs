using Model;
using Telemetry.DataTypes;
using UnityEngine;
using Grid = Model.Grid;

namespace Telemetry.Events
{
    public sealed class TowerEvent : TelemetryEventTriggerBase
    {
        [SerializeField] private TowerPlacement tower;

        protected override string dataName()
        {
            return "Tower";
        }

        protected override TelemetryDataBase ProcessEvent()
        {
            //TODO Refactor to use helper functions
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(this.tower.GetMousePosition());
            Cell cell = mouseWorldPos.ToCell();
            Grid grid = this.tower.GetGrid();
            return new TowerData(cell.ToVector2(), 
                grid.IsCellOnPath(cell), 
                grid.IsCellOccupiedByTower(cell),
                grid.IsCellOnEnemyCastle(cell),
                grid.IsCellOnPlayerCastle(cell),
                grid.IsCellOnCenterWall(cell));
        }
    }
}