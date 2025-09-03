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
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
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
builder.Services.AddSingleton<WebSocketConnectionManager>();
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
app.UseCors("AllowAll");

/// <summary>
/// WebSocket 端点
/// 客户端连接地址: ws://localhost:5000/ws?token=my-secret
/// </summary>
app.Map("/ws", async (HttpContext context, WebSocketConnectionManager connectionManager) =>
{
    // 检查是否为 WebSocket 请求
    if (!context.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        return;
    }


    var token = context.Request.Query["token"];
    if (string.IsNullOrEmpty(token) || token != "my-secret")
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return;
    }

    // 接受 WebSocket 连接
    var socket = await context.WebSockets.AcceptWebSocketAsync();
    var id = connectionManager.AddSocket(socket);

    var buffer = new byte[1024 * 4];
    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

    // 循环接收客户端消息
    while (!result.CloseStatus.HasValue)
    {
        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        Console.WriteLine($"收到客户端消息: {message}");

        // 回显消息
      //  await socket.SendAsync(Encoding.UTF8.GetBytes($"Echo: {message}"), WebSocketMessageType.Text, true, CancellationToken.None);

        // 继续接收
        result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    }

    // 客户端关闭连接
    await connectionManager.RemoveSocketAsync(id);
});
app.Run();

