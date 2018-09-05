using Huanent.Logging.File.UI.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Huanent.Logging.File.UI
{
    public class ConfigService
    {
        private readonly LoggingFileUIOptions _options;
        private readonly string _path;

        public ConfigService(LoggingFileUIOptions options)
        {
            _options = options;

            LogDir = LogDir = options.Path ?? Path.Combine(AppContext.BaseDirectory, "logs");

            if (!Directory.Exists(LogDir))
            {
                Directory.CreateDirectory(LogDir);
            }

            _path = Path.Combine(LogDir, "config.json");
        }

        public string LogDir { get; private set; }

        public ConfigModel Get()
        {
            if (!System.IO.File.Exists(_path)) return null;
            return JsonConvert.DeserializeObject<ConfigModel>(System.IO.File.ReadAllText(_path));
        }

        public void Save(ConfigModel config)
        {
            System.IO.File.WriteAllText(_path, JsonConvert.SerializeObject(config));
        }
    }
}
