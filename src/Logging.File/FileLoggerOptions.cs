using System;
using System.Collections.Generic;
using System.Text;

namespace Huanent.Logging.File;

public class FileLoggerOptions
{
    public string Path { get; set; } = "logs";
    public string DateFormat { get; set; } = "yyyyMMdd";
}