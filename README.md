## Microsoft.Extensions.Logging文件文本日志拓展
你可通过 Install-Package Huanent.Logging.File来安装拓展，或者在nuget包浏览器搜索Huanent.Logging.File
安装配置完插件后，你的asp.net core程序会将输出的日志保存在程序根目录下的logs文件夹，并以日期划分文件名
#### 配置
1. 安装Huanent.Logging.File

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
手动高亮  -->    "Default": "Wanring" //具体输入级别自行修改，也可添加详细的分类别输出
手动高亮  -->    }
手动高亮  --> }
            }
}
```

配置完成
