using Telemetry.DataTypes;
using UnityEngine;

namespace Telemetry.Events
{
    public sealed class MouseCursorEvent : TelemetryEventContinuousBase
    {
        protected override TelemetryDataBase ProcessEvent()
        {
            return new Position2DData(new Vector2(this.transform.position.x, this.transform.position.y));
        }
    }
}