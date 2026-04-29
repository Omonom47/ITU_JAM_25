using System.IO;
using UnityEngine;

namespace Telemetry.DataTypes
{
    public sealed class TowerData : TelemetryDataBase
    {
        private readonly Vector2 position;
        private readonly bool onRoad, onOtherTower, onOpponentsCastle, onPlayerCastle, onCenterWall;

        public TowerData(Vector2 position, bool onRoad, bool onOtherTower, bool onOpponentsCastle,
            bool onPlayerCastle, bool onCenterWall)
        {
            this.position = position;
            this.onRoad = onRoad;
            this.onOtherTower = onOtherTower;
            this.onOpponentsCastle = onOpponentsCastle;
            this.onPlayerCastle = onPlayerCastle;
            this.onCenterWall = onCenterWall;
        }

        public override string GetColumnNames()
        {
            return "Position;OnRoad;OnOtherTower;OnOpponentsCastle;OnPlayerCastle;OnCenterWall";
        }

        public override string GetColumnValues()
        {
            return
                $"{this.position};{this.onRoad};{this.onOtherTower};{this.onOpponentsCastle};{this.onPlayerCastle};{this.onCenterWall}";
        }

        public override void WriteToFile(StreamWriter writer)
        {
            writer.Write(this.GetColumnValues());
        }
    }
}