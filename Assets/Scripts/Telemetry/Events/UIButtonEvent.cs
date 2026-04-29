using Telemetry.DataTypes;
using UnityEngine;

namespace Telemetry.Events
{
    public sealed class UIButtonEvent : TelemetryEventTriggerBase
    {
        [SerializeField] private string id;

        protected override string dataName()
        {
            return "UIButton";
        }

        protected override TelemetryDataBase ProcessEvent()
        {
            return new UIButtonClickData(this.turnManager.TurnNumber, this.id, this.turnManager.Phase);
        }
    }
}