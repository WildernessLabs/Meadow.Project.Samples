using Meadow;
using System;
using System.IO;
using YamlDotNet.Serialization;

namespace MeadowWifi
{
    public static class ConfigFileManager
    {
        public static void CreateConfigFiles(string ssid, string password) 
        {
            CreateMeadowConfigFile();
            CreateWifiConfigFile(ssid, password);
        }

        public static void DeleteConfigFiles()
        {
            DeleteMeadowConfigFile();
            DeleteWifiConfigFile();
        }

        private static void CreateMeadowConfigFile()
        {
            try
            {
                var configFile = new MeadowConfigFile();

                var serializer = new SerializerBuilder().Build();
                var yaml = serializer.Serialize(configFile);

                using (var fs = File.CreateText(Path.Combine(MeadowOS.FileSystem.UserFileSystemRoot, "meadow.config.yaml")))
                {
                    fs.WriteLine(yaml);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void CreateWifiConfigFile(string ssid, string password)
        {
            try
            {
                var configFile = new WifiConfigFile(ssid, password);

                var serializer = new SerializerBuilder().Build();
                var yaml = serializer.Serialize(configFile);

                using (var fs = File.CreateText(Path.Combine(MeadowOS.FileSystem.UserFileSystemRoot, "wifi.config.yaml")))
                {
                    fs.WriteLine(yaml);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void DeleteMeadowConfigFile() 
        {
            string path = $"{MeadowOS.FileSystem.UserFileSystemRoot}meadow.config.yaml";

            Console.WriteLine(path);

            File.Delete(path);
        }

        private static void DeleteWifiConfigFile()
        {
            string path = $"{MeadowOS.FileSystem.UserFileSystemRoot}wifi.config.yaml";

            Console.WriteLine(path);

            File.Delete(path);
        }
    }
}