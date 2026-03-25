using UnityEngine;

namespace Telemetry
{
    public abstract class TelemetryEventTriggerBase : MonoBehaviour
    {
        [SerializeField] private string dataName;

        private void Start()
        {
            Telemetry.Reserve(this.dataName, this);
        }

        public void TriggerEvent()
        {
            TelemetryDataBase newData = this.ProcessEvent();

            if (newData == null)
            {
                Debug.LogError("Data should not be null.");
                return;
            }

            Telemetry.AddToData(this.dataName, this, newData);
        }

        protected abstract TelemetryDataBase ProcessEvent();
    }
}