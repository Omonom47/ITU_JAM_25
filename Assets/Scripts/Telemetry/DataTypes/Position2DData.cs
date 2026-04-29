using System.IO;
using UnityEngine;

namespace Telemetry.DataTypes
{
    public sealed class Position2DData : TelemetryDataBase
    {
        private readonly Vector2 position;

        public Position2DData(Vector2 position)
        {
            this.position = position;
        }

        public override string GetColumnNames()
        {
            return "Position";
        }

        public override string GetColumnValues()
        {
            return this.position.ToString();
        }

        public override void WriteToFile(StreamWriter writer)
        {
            writer.Write(this.position);
        }
    }
}