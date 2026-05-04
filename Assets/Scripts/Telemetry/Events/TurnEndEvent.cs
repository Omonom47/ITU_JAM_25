using ScriptableObjects;
using Telemetry.DataTypes;
using UnityEngine;

namespace Telemetry.Events
{
    public sealed class TurnEndEvent : TelemetryEventTriggerBase
    {
        [SerializeField] IntVariable playerHealth, enemyHealth;

        protected override string dataName()
        {
            return "TurnEnd";
        }

        protected override TelemetryDataBase ProcessEvent()
        {
            return new HealthData(this.turnManager.TurnNumber, this.playerHealth.Value, this.enemyHealth.Value);
        }
    }
}