using Telemetry.DataTypes;

namespace Telemetry.Events
{
    public sealed class UIButtonEvent : TelemetryEventTriggerBase
    {
        protected override TelemetryDataBase ProcessEvent()
        {
            return new UIButtonClickData();
        }
    }
}