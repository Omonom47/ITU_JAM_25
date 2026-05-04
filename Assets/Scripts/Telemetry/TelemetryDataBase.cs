using System.IO;

namespace Telemetry
{
    public abstract class TelemetryDataBase
    {
        public abstract string GetColumnNames();
        
        public abstract string GetColumnValues();
        
        public abstract void WriteToFile(StreamWriter writer);
        
    }
}
