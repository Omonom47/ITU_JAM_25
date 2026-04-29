using Model;
using Telemetry.DataTypes;
using UnityEngine;

namespace Telemetry.Events
{
    public sealed class TowerEvent : TelemetryEventTriggerBase
    {
        [SerializeField] private TowerPlacement tower;
        [SerializeField] private TurnManager turnManager;

        protected override string dataName()
        {
            return "Tower";
        }

        protected override TelemetryDataBase ProcessEvent()
        {
            //TODO Refactor to use helper functions
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(this.tower.GetMousePosition());
            Cell cell = mouseWorldPos.ToCell();
            return new TowerData(cell.ToVector2(), false, false, false, false, false);
        }
    }
}