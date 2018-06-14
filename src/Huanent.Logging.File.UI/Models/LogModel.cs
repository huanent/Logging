using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Huanent.Logging.File.UI.Models
{
    public class LogModel
    {
        public DateTime Time { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public string Loglevel { get; set; }
    }
}
