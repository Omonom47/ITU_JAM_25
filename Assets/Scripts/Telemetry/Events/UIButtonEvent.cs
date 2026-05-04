using Telemetry.DataTypes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Telemetry.Events
{
    public sealed class UIButtonEvent : TelemetryEventTriggerBase,IPointerDownHandler
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

        public void OnPointerDown(PointerEventData eventData)
        {
            this.ProcessEvent();
        }
    }
}