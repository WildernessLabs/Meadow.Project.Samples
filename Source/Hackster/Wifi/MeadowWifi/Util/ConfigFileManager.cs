using Meadow;
using System;
using System.IO;
using YamlDotNet.Serialization;

namespace MeadowWifi
{
    public static class ConfigFileManager
    {
        public static void CreateMeadowConfigFile()
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

        public static void CreateWifiConfigFile(string ssid, string password)
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
    }
}