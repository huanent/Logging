# Microsoft.Extensions.Logging 日志组件拓展
- 文件文本日志
- 文件文本日志UI插件
- 自定义介质日志

## Microsoft.Extensions.Logging.File文件文本日志
安装配置完插件后，你的asp.net core程序会将输出的日志保存在程序根目录下的logs文件夹，并以日期划分文件名
#### 配置
1. 安装Huanent.Logging.File nuget包

2. 在Program.cs文件中添加
```
 public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
   手动高亮  --> .ConfigureLogging(builder => builder.AddFile()) 
                .Build();
```
3. 配置appsettings.json文件,添加File节点
```
{
            "Logging": {
              "IncludeScopes": false,
              "Debug": {
                "LogLevel": {
                  "Default": "Warning"
                }
              },
              "Console": {
                "LogLevel": {
                  "Default": "Warning"
                }
              },
手动高亮  -->  "File": {
手动高亮  -->    "LogLevel": {
手动高亮  -->    "Default": "Warning" //具体输入级别自行修改，也可添加详细的分类别输出
手动高亮  -->    }
手动高亮  --> }
            }
}
```
步骤3可忽略，如果忽略会将所有类别日志都输出到文本文件

## Microsoft.Extensions.Logging.File.UI
1. 安装Huanent.Logging.File.UI nuget包
2. 修改Startup.cs
```
 public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
手动高亮 --> services.AddLoggingFileUI();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
手动高亮 -->   app.UseStaticFiles();
手动高亮 -->   app.UseMvc();
        }
    }
```
3. 启动网站，访问页面http://xxxxx:xx/logging 即可打开日志查看网页

## Microsoft.Extensions.Logging.Abstract 自定义介质日志
可以通过实现ILoggerWriter来自定义日志输出保存的介质
#### 配置

1. 安装Huanent.Logging.Abstract nuget包

2. 实现ILoggerWriter
```
public class MyLogWriter : ILoggerWriter
    {
        public void WriteLog(LogLevel level, string message, string name, Exception exception, EventId eventId)
        {
          //在此处自定义日志的保存方式。可以保存到数据库，云等。。。
          //注意！MyLogWriter对象在DI容器中是单例形式存在的！
          //注意！请勿在MyLogWriter中进行会日志输出的操作，那可能会导致循环递归，栈溢出！
        }
    }
```
3. 在Program.cs文件中添加
```
 public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
   手动高亮  --> .ConfigureLogging(builder => builder.AddAbstract<MyLogWriter>()) 
                .Build();
```
4. 配置appsettings.json文件,添加Abstract节点
```
{
            "Logging": {
              "IncludeScopes": false,
              "Debug": {
                "LogLevel": {
                  "Default": "Warning"
                }
              },
              "Console": {
                "LogLevel": {
                  "Default": "Warning"
                }
              },
手动高亮  -->  "Abstract": {
手动高亮  -->    "LogLevel": {
手动高亮  -->    "Default": "Warning" //具体输入级别自行修改，也可添加详细的分类别输出
手动高亮  -->    }
手动高亮  --> }
            }
}
```
步骤4可忽略，如果忽略会将所有类别日志都输出
