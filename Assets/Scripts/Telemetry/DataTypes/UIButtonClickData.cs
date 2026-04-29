using System.IO;

namespace Telemetry.DataTypes
{
    public sealed class UIButtonClickData : TelemetryDataBase
    {
        private readonly int turn;
        private readonly string id;
        private readonly TurnManager.TurnPhase phase;

        public UIButtonClickData(int turn, string id, TurnManager.TurnPhase phase)
        {
            this.turn = turn;
            this.id = id;
            this.phase = phase;
        }

        public override string GetColumnNames()
        {
            return "Turn;Name;Phase";
        }

        public override string GetColumnValues()
        {
            return $"{this.turn};{this.id};{this.phase}";
        }

        public override void WriteToFile(StreamWriter writer)
        {
            writer.Write(this.GetColumnValues());
        }
    }
}