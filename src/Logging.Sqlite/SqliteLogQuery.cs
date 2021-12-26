using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Huanent.Logging.Sqlite
{
    public class SqliteLogQueryParams
    {
        public LogLevel? Level { get; set; }
        public DateTimeOffset StartDateTime { get; set; } = DateTimeOffset.UtcNow.AddDays(-1);
        public DateTimeOffset EndDateTime { get; set; } = DateTimeOffset.UtcNow;
        public string Name { get; set; }
        public string Keyword { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        
        internal long SkipCount => (PageIndex - 1) * PageSize;
    }

    public class SqliteLogQueryResult
    {
        public int PageIndex { get; internal set; }
        public int PageSize { get; internal set; }
        public long LogCount { get; internal set; }
        public int PageCount => Convert.ToInt32((LogCount + PageSize - 1) / PageSize);
        public IEnumerable<SqliteLog> Logs { get; internal set; }
    }
}