using Meadow;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace MeadowMapleTemperature.Models
{
    public class SQLiteDatabaseManager
    {
        SQLiteConnection Database { get; set; }

        private static readonly Lazy<SQLiteDatabaseManager> instance =
            new Lazy<SQLiteDatabaseManager>(() => new SQLiteDatabaseManager());

        public static SQLiteDatabaseManager Instance
        {
            get { return instance.Value; }
        }

        private SQLiteDatabaseManager()
        {
            var databasePath = Path.Combine(MeadowOS.FileSystem.DataDirectory, "ClimateReadings.db");
            Database = new SQLiteConnection(databasePath);

            CreateTables();
        }

        bool isConfigured = false;
        protected void CreateTables()
        {
            Console.WriteLine("ConfigureDatabase");
            Database.CreateTable<TemperatureModel>();
            Console.WriteLine("Table created");
            isConfigured = true;
        }

        public bool SaveReading(TemperatureModel temperature)
        {
            if (isConfigured == false)
            {
                Console.WriteLine("SaveUpdateReading: DB not ready");
                return false;
            }

            if (temperature == null)
            {
                Console.WriteLine("SaveUpdateReading: Conditions is null");
                return false;
            }

            Console.WriteLine("Saving climate reading to DB");

            Database.Insert(temperature);

            Console.WriteLine($"Successfully saved to database");

            return true;
        }

        public List<TemperatureModel> GetTemperatureReadings()
        {
            return Database.Table<TemperatureModel>().ToList();
        }
    }
}