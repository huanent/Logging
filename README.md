# Huanent.Logging
microsoft.extensions.logging日子组件拓展

# 使用说明
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

loglevel表示要记录的最小日子级别
