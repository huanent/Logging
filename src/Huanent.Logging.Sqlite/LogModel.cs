using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Huanent.Logging.Sqlite
{
    public class LogModel
    {
        public string Category { get; set; }

        public DateTime DateTime { get; set; }

        public string Exception { get; set; }

        public int Id { get; set; }

        public string LogLevel { get; set; }

        public string Massage { get; set; }
    }
}
