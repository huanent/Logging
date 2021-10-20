using System;
using System.Collections.Generic;
using System.Text;

namespace Huanent.Logging.File
{
    public class FileLogOptions
    {
        public string Path { get; set; } = "logs";
        public string DateFormat { get; set; } = "yyyMMdd";
    }
}
