using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Logging.TextWriter
{
    public class FileWriterLoggerOptions : IOptions<FileWriterLoggerOptions>
    {
        /// <summary>
        /// 日志所在文件夹绝对路径
        /// </summary>
        public string BasePath { get; set; } = AppContext.BaseDirectory;

        /// <summary>
        /// 日志记录级别
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        public FileWriterLoggerOptions Value => this;
    }
}