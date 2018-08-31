using Huanent.Logging.File.UI.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Huanent.Logging.File.UI
{
    public class ConfigService
    {
        private readonly string _dir;
        private readonly LoggingFileUIOptions _options;
        private readonly string _path;

        public ConfigService(LoggingFileUIOptions options)
        {
            _options = options;

            _dir = _dir = options.Path ?? Path.Combine(AppContext.BaseDirectory, "logs");

            if (!Directory.Exists(_dir))
            {
                Directory.CreateDirectory(_dir);
            }

            _path = Path.Combine(_dir, "config.json");
        }

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