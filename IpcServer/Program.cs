using System.Net.WebSockets;
using System.Text;
using IpcServer;
using NLog.Web;
using IpcServer.Domain;
using IpcServer.Domain.Interfaces;
using IpcServer.Infrastructure;
using IpcServer.WorkFlow.Devices;
using IpcServer.WorkFlow.Workflows;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// 添加 CORS 服务
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // 你的前端地址
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddSingleton<SqlSugarDbContext>(); // 单例 DbContext
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
Console.WriteLine("连接字符串: " + builder.Configuration.GetConnectionString("Default"));
//添加DI
builder.Services.AddScoped<LeakTestAction>();
builder.Services.AddScoped<ScanAction>();
builder.Services.AddScoped<TightenAction>();
builder.Services.AddScoped<IRecipeExecutor, RecipeExecutor>();



// 注册控制器
builder.Services.AddControllers();
// 清除默认日志提供程序，使用 NLog
builder.Logging.ClearProviders();
builder.Host.UseNLog();
//添加背景服务
builder.Services.AddHostedService<HostedService>();
// 添加 Swagger 服务
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//注册websocket服务
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    
}
// 启用 WebSocket
app.UseWebSockets();
// 映射 API 控制器
app.MapControllers();


app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.MapHub<WorkHub>("/workHub").RequireCors("AllowFrontend");;

app.Run();

