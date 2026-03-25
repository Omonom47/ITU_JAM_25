using UnityEngine;

namespace Telemetry
{
    public abstract class TelemetryEventContinuousBase : MonoBehaviour
    {
        [SerializeField] private string dataName;
        [Min(0.1f)] [SerializeField] private float secondsBetweenEvents = 0.1f;

        private float currentTime;

        private void Start()
        {
            Telemetry.Reserve(this.dataName, this);
        }

        private void Update()
        {
            this.currentTime += Time.deltaTime;

            if (this.currentTime < this.secondsBetweenEvents)
                return;

            this.currentTime -= this.secondsBetweenEvents;
            this.TriggerEvent();
        }

        private void TriggerEvent()
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