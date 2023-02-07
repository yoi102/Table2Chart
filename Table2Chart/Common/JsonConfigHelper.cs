using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace Table2Chart.Common
{
    /// <summary>
    /// 用于序列化
    /// </summary>
    public static class JsonConfigHelper
    {
        public enum ConfigFile
        {
            SkinConfig,
            VariableConfig,
        }

        public static readonly string ConfigFolder = "Configs";
        public static readonly string SkinConfig = ConfigFolder + "/SkinConfig.config";
        public static readonly string VariableConfig = ConfigFolder + "/VariableConfig.config";

        private static string GetFilePathByConfigFileEnum(ConfigFile configFile)
        {
            string file = string.Empty;

            switch (configFile)
            {
                case ConfigFile.SkinConfig:
                    file = SkinConfig;
                    break;

                case ConfigFile.VariableConfig:
                    file = VariableConfig;
                    break;

                default:
                    break;
            }
            return file;
        }

        public static void ReadConfig<T>(ref T obj, ConfigFile configFile)
        {
            if (!Directory.Exists(ConfigFolder))
                return;
            string file = GetFilePathByConfigFileEnum(configFile);
            if (string.IsNullOrEmpty(file))
                return;

            if (!File.Exists(file))
                return;
            try
            {
                obj = JsonConvert.DeserializeObject<T>(File.ReadAllText(path: file), new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented,
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                    DateParseHandling = DateParseHandling.DateTime
                });
            }
            catch
            {
            }
        }

        public static void SaveConfig<T>(T obj, ConfigFile configFile)
        {
            if (!Directory.Exists(ConfigFolder))
            {
                Directory.CreateDirectory(ConfigFolder);
            }

            string file = GetFilePathByConfigFileEnum(configFile);
            if (string.IsNullOrEmpty(file))
                return;

            try
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new JavaScriptDateTimeConverter());
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.TypeNameHandling = TypeNameHandling.Auto;
                serializer.Formatting = Formatting.Indented;
                serializer.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                serializer.DateParseHandling = DateParseHandling.DateTime;
                using (StreamWriter sw = new StreamWriter(file))
                using (JsonWriter writer = new JsonTextWriter(sw))
                    serializer.Serialize(writer, obj);
            }
            catch
            {
            }
        }
    }
}