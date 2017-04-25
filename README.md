# Huanent.Logging
microsoft.extensions.logging日子组件拓展

# Huanent.Loggin使用说明(使用此组件就无需再安装配置Huanent.Logging.File组件)
1.安装nuget包 Huanent.Logging.File 

2.在setup文件ConfigureServices方添加注册
```
services.AddOptions();
services.Configure<LoggingOptions>(Configuration.GetSection("LoggingOptions"));
```

3. 在setup文件Configure方添加注册
```
app.UseLogging();
```

4在appsettings.json添加节点
```
  "LoggingOptions": {
    "ConsoleLogLevel": "Information",
    "DebugLogLevel": "Trace",
    "FileLogLevel": "Warning"
  },
```

# Huanent.Logging.File使用说明
1.安装nuget包 Huanent.Logging.File 

2.在setup文件Configure方添加注册
```
loggerFactory.AddFileLog(new FileWriterLoggerOptions
{
       BasePath = $"{env.ContentRootPath}\\log",
       LogLevel = Loglevel.Warring
});
```
basePath设置文件要存储的路径

loglevel表示要记录的最小日志级别
