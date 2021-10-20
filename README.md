# Microsoft.Extensions.Logging extensions

## Usage

### Huanent.Logging.Core

```
public class ConsoleLogWriter : ILogWriter
{
    public void WriteLog(LogLevel level, string message, string name, Exception exception, EventId eventId)
    {
        Console.WriteLine($"From {nameof(ConsoleLogWriter)} {level} {message} {name} {exception} {eventId}");
    }
}

//ILoggingBuilder
logging.AddImplementation<ConsoleLogWriter>();
```

### Huanent.Logging.File

```
//ILoggingBuilder
logging.AddFile();
```
Log will out in {application folder}/logs/20211020.txt
```
[Error] [LoggingFileSample.Worker] [0] [2021/10/20 01:05:35 +00:00]
Worker running at: 10/20/2021 21:05:35 +08:00
System.Exception: error

[Error] [LoggingFileSample.Worker] [0] [2021/10/20 01:05:36 +00:00]
Worker running at: 10/20/2021 21:05:36 +08:00
System.Exception: error

[Error] [LoggingFileSample.Worker] [0] [2021/10/20 01:05:37 +00:00]
Worker running at: 10/20/2021 21:05:37 +08:00
System.Exception: error
```
