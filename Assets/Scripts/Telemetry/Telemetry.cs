using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Telemetry
{
    public sealed class Telemetry
    {
        private static Telemetry instance;

        private readonly Dictionary<int, TelemetryData> data = new Dictionary<int, TelemetryData>();

        private static Telemetry GetInstance()
        {
            instance ??= new Telemetry();

            return instance;
        }

        public static void Reserve(string dataName, MonoBehaviour owner)
        {
            int ownerHash = owner.gameObject.GetInstanceID();
            int nameHash = dataName.GetHashCode();

            Telemetry telemetry = GetInstance();

            if (telemetry.data.ContainsKey(nameHash))
            {
                Debug.LogError($"Duplicate telemetry name: {nameHash}", owner);
                return;
            }

            TelemetryData telemetryData = new TelemetryData(dataName, ownerHash);
            telemetry.data.Add(nameHash, telemetryData);
        }

        public static void AddToData(string dataName, MonoBehaviour owner, TelemetryDataBase newData)
        {
            int ownerHash = owner.gameObject.GetInstanceID();
            int nameHash = dataName.GetHashCode();

            Telemetry telemetry = GetInstance();

            if (!telemetry.data.TryGetValue(nameHash, out TelemetryData telemetryData))
            {
                Debug.Log("Couldn't find telemetry data", owner);
                return;
            }

            if (!telemetryData.CompareOwner(ownerHash))
            {
                Debug.LogError("Owner mismatch", owner);
                return;
            }

            telemetryData.AddData(newData);
        }

        public static void SaveDataToFile()
        {
            DateTime time = DateTime.Now;
            string fileName = $"{time:MM_dd_HH_mm_ss}.txt";
            Debug.Log($"File name: {fileName}");
            string path = Path.Combine(Application.persistentDataPath, fileName);
            Debug.Log($"Saving data to {path}");

            if (!File.Exists(path))
                File.Create(path).Close();
            else
            {
                Debug.LogError("File already exists");
                return;
            }

            StreamWriter writer = new StreamWriter(path, true);

            Telemetry telemetry = GetInstance();
            foreach (TelemetryData telemetryData in telemetry.data.Values)
                telemetryData.WriteDataToFile(writer);

            writer.Close();
        }
    }

    internal readonly struct TelemetryData
    {
        private readonly string name;
        private readonly int ownerHash;
        private readonly List<TelemetryDataBase> data;
        private readonly List<DateTime> dataTime;

        public TelemetryData(string name, int ownerHash) : this()
        {
            this.name = name;
            this.ownerHash = ownerHash;
            this.data = new List<TelemetryDataBase>();
            this.dataTime = new List<DateTime>();
        }

        public bool CompareOwner(int hash)
        {
            return this.ownerHash == hash;
        }

        public void AddData(TelemetryDataBase newData)
        {
            this.data.Add(newData);
            this.dataTime.Add(DateTime.Now);
        }

        public void WriteDataToFile(StreamWriter writer)
        {
            writer.WriteLine(this.name);
            writer.WriteLine("{");

            for (int i = 0; i < this.data.Count; i++)
            {
                TelemetryDataBase telemetryData = this.data[i];
                DateTime telemetryTime = this.dataTime[i];
                writer.WriteLine("{");
                telemetryData.WriteToFile(writer);
                writer.WriteLine("");
                writer.WriteLine(telemetryTime.ToString("HH:mm:ss"));
                writer.WriteLine("}");
            }

            writer.WriteLine("}");
        }
    }
}