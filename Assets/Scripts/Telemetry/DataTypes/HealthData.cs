using System.IO;

namespace Telemetry.DataTypes
{
    public sealed class HealthData : TelemetryDataBase
    {
        private readonly int turn, playerHealth, enemyHealth;

        public HealthData(int turn, int playerHealth, int enemyHealth)
        {
            this.turn = turn;
            this.playerHealth = playerHealth;
            this.enemyHealth = enemyHealth;
        }

        public override string GetColumnNames()
        {
            return "Turn;Player;Enemy";
        }

        public override string GetColumnValues()
        {
            return $"{this.turn};{this.playerHealth};{this.enemyHealth}";
        }

        public override void WriteToFile(StreamWriter writer)
        {
            writer.Write(this.GetColumnValues());
        }
    }
}