using Logging.TextWriter;
using System.Diagnostics;
using System.IO;

namespace Microsoft.Extensions.Logging
{
    public static class AddFileLogExtention
    {
        /// <summary>
        /// 添加文件日志
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="option"></param>
        public static void AddFileLog(this ILoggerFactory factory, FileWriterLoggerOptions option = null)
        {
            if (option == null) option = new FileWriterLoggerOptions();
            if (!Directory.Exists(option.BasePath))
            {
                Directory.CreateDirectory(option.BasePath);
            }

            var sourceSwitch = GetSourceSwitch(option);
            var listener = new FileWriterTraceListener(option.BasePath);
            factory.AddTraceSource(sourceSwitch, listener);
        }

        private static SourceSwitch GetSourceSwitch(FileWriterLoggerOptions option)
        {
            var sourceSwitch = new SourceSwitch("default");
            switch (option.LogLevel)
            {
                case LogLevel.Trace:
                    sourceSwitch.Level = SourceLevels.All;
                    break;

                case LogLevel.Debug:
                    sourceSwitch.Level = SourceLevels.All;
                    break;

                case LogLevel.Information:
                    sourceSwitch.Level = SourceLevels.Information;
                    break;

                case LogLevel.Warning:
                    sourceSwitch.Level = SourceLevels.Warning;
                    break;

                case LogLevel.Error:
                    sourceSwitch.Level = SourceLevels.Error;
                    break;

                case LogLevel.Critical:
                    sourceSwitch.Level = SourceLevels.Critical;
                    break;

                case LogLevel.None:
                    sourceSwitch.Level = SourceLevels.Off;
                    break;

                default:
                    break;
            }

            return sourceSwitch;
        }
    }
}