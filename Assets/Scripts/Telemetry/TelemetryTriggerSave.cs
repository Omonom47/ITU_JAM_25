using UnityEngine;
using UnityEngine.InputSystem;

namespace Telemetry
{
    public sealed class TelemetryTriggerSave : MonoBehaviour
    {
        private InputSystem_Actions inputSystemActions;

        private void Start()
        {
            this.inputSystemActions = new InputSystem_Actions();
            this.inputSystemActions.Telemetry.TriggerSave.performed += this.Check;

            this.inputSystemActions.Enable();
        }

        private void OnDestroy()
        {
            this.inputSystemActions.Disable();
        }

        private void Check(InputAction.CallbackContext ctx)
        {
            Telemetry.SaveDataToFile();
        }
    }
}