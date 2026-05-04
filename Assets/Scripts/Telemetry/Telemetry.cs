using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Telemetry
{
    public sealed class Telemetry
    {
        private static Telemetry instance;

        private readonly Dictionary<int, TelemetryData> data = new Dictionary<int, TelemetryData>();

        private static string fileName = "Empty";

        private static Telemetry GetInstance()
        {
            instance ??= new Telemetry();

            return instance;
        }

        public static void SetFileName(string name)
        {
            fileName = name;
        }


        public static void AddToData(string dataName, TelemetryDataBase newData)
        {
            int nameHash = dataName.GetHashCode();

            Telemetry telemetry = GetInstance();

            if (telemetry.data.TryGetValue(nameHash, out TelemetryData telemetryData))
            {
                telemetryData.AddData(newData);
                return;
            }

            TelemetryData toAdd = new TelemetryData(dataName);
            toAdd.AddData(newData);
            telemetry.data.Add(nameHash, toAdd);
        }

        public static void SaveDataToFile()
        {
            string fileNameWithType = $"{fileName}.txt";
            Debug.Log($"File name: {fileNameWithType}");
            string commonFilePath = Path.Combine(Application.persistentDataPath, fileNameWithType);
            Debug.Log($"Saving data to {commonFilePath}");

            if (!File.Exists(commonFilePath))
                File.Create(commonFilePath).Close();
            else
            {
                Debug.LogError("File already exists");
                return;
            }

            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, fileName)))
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, fileName));

            StreamWriter commonWriter = new StreamWriter(commonFilePath, true);

            Telemetry telemetry = GetInstance();
            foreach (TelemetryData telemetryData in telemetry.data.Values)
            {
                try
                {
                    telemetryData.WriteDataToCommonFile(commonWriter);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                string csvFilePath =
                    Path.Combine(Application.persistentDataPath, fileName, $"{telemetryData.GetName()}.csv");
                File.Create(csvFilePath).Close();

                StreamWriter csvWriter = new StreamWriter(csvFilePath, true);
                try
                {
                    telemetryData.WriteToCSVFile(csvWriter);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                csvWriter.Close();
            }

            commonWriter.Close();
        }
    }

    internal readonly struct TelemetryData
    {
        private readonly string name;
        private readonly List<TelemetryDataBase> data;
        private readonly List<DateTime> dataTime;

        public TelemetryData(string name) : this()
        {
            this.name = name;
            this.data = new List<TelemetryDataBase>();
            this.dataTime = new List<DateTime>();
        }


        public void AddData(TelemetryDataBase newData)
        {
            this.data.Add(newData);
            this.dataTime.Add(DateTime.Now);
        }

        public void WriteDataToCommonFile(StreamWriter writer)
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

        public void WriteToCSVFile(StreamWriter writer)
        {
            if (this.data.Count == 0)
                return;

            StringBuilder sb = new StringBuilder("Time;" + this.data[0].GetColumnNames());

            for (int i = 0; i < this.data.Count; i++)
            {
                sb.Append('\n').Append(this.dataTime[i].ToString("HH:mm:ss:fff")).Append(";")
                    .Append(this.data[i].GetColumnValues());
            }

            writer.WriteLine(sb.ToString());
        }

        public string GetName()
        {
            return this.name;
        }
    }
}