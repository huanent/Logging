using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Huanent.Logging.File
{
    class FileWriterTraceListener : TextWriterTraceListener
    {
        string _basePath;

        /// <summary>
        /// 文件日志写入器
        /// </summary>
        /// <param name="basePath">文件存放根目录</param>
        public FileWriterTraceListener(string basePath)
        {
            _basePath = basePath;
        }

        public override void Write(string message)
        {
            var msgBuilder = new StringBuilder();
            msgBuilder.Append("-----");
            msgBuilder.Append(DateTime.Now.ToString("HH:mm:ss"));
            msgBuilder.AppendLine("-------------------------------------");
            msgBuilder.Append(message);
            System.IO.File.AppendAllText($"{_basePath}\\{DateTime.Now.Date.ToString("yyyMMdd")}.txt", msgBuilder.ToString());
        }

        public override void WriteLine(string message)
        {
            var msgBuilder = new StringBuilder();
            msgBuilder.AppendLine(message);
            msgBuilder.AppendLine();
            System.IO.File.AppendAllText($"{_basePath}\\{DateTime.Today.ToString("yyyMMdd")}.txt", msgBuilder.ToString());
        }
    }
}