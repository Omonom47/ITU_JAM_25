using System;
using Telemetry.DataTypes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Telemetry.Events
{
    public sealed class MouseCursorEvent : TelemetryEventContinuousBase
    {
        private Vector2 _mousePosition;
        private InputSystem_Actions _inputSystemActions;

        private void OnEnable()
        {
            this._inputSystemActions = new InputSystem_Actions();
            this._inputSystemActions.Player.Enable();
            this._inputSystemActions.Player.Look.performed += this.look;
        }

        private void OnDisable()
        {
            this._inputSystemActions.Player.Disable();
        }

        private void look(InputAction.CallbackContext context)
        {
            this._mousePosition = context.action.ReadValue<Vector2>();
        }

        protected override string dataName()
        {
            return "MouseCursor";
        }

        protected override TelemetryDataBase ProcessEvent()
        {
            return new Position2DData(this._mousePosition);
        }
    }
}