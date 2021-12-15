using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

//Use File logWriter
builder.Logging.AddSqlite(options =>
{
    options.Path = "custom_log_folder/log.db";
});

var app = builder.Build();

app.MapGet("/", ([FromServices] ILogger<Program> logger) =>
{
    logger.LogError("Logging from sqlite logging perovider");
});

app.Run();