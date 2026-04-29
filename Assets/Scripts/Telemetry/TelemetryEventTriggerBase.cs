using UnityEngine;

namespace Telemetry
{
    public abstract class TelemetryEventTriggerBase : MonoBehaviour
    {
        [SerializeField] protected TurnManager turnManager;
        protected abstract string dataName();

        public void TriggerEvent()
        {
            TelemetryDataBase newData = this.ProcessEvent();

            Debug.Log(newData);
            if (newData == null)
            {
                Debug.LogError("Data should not be null.");
                return;
            }

            Telemetry.AddToData(this.dataName(), newData);
        }

        protected abstract TelemetryDataBase ProcessEvent();
    }
}