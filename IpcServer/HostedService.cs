using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;

namespace IpcServer;

public class HostedService:BackgroundService
{
    private readonly WebSocketConnectionManager _webSocketConnection;
    private readonly ILogger<HostedService> _logger;

    public HostedService(WebSocketConnectionManager webSocketConnection,ILogger<HostedService> logger)
    {
        _webSocketConnection = webSocketConnection;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       

        while (!stoppingToken.IsCancellationRequested)
        {
            //推送的消息
            var payload = new
            {
                time = DateTime.Now.ToString("HH:mm:ss"),
                type = "heartbeat",
                message = "服务器推送"
            };
            var message = JsonConvert.SerializeObject(payload);
            var buffer = Encoding.UTF8.GetBytes(message);
            // 遍历所有连接并推送
            foreach (var socket in _webSocketConnection.GetAllSockets())
            {
                if (socket.State == WebSocketState.Open)
                {
                    try
                    {
                        await socket.SendAsync(buffer, WebSocketMessageType.Text, true, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(ex, "推送消息失败");
                    }
                }
            }
            // 每 5 秒推送一次
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }

    }
}