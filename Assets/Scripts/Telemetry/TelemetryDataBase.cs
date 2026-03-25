using System.IO;

namespace Telemetry
{
    public abstract class TelemetryDataBase
    {
        public abstract void WriteToFile(StreamWriter writer);
    }
}
