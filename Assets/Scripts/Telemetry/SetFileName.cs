using UnityEngine;

namespace Telemetry
{
    public sealed class SetFileName : MonoBehaviour
    {
        public void SetName(string fileName) =>
            Telemetry.SetFileName(fileName);
    }
}