using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace IpcServer;

public class WebSocketConnectionManager
{
    // 使用线程安全的字典保存连接（Key: 连接ID, Value: WebSocket对象）
    private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();

    /// <summary>
    /// 添加一个新的 WebSocket 连接
    /// </summary>
    public string AddSocket(WebSocket socket)
    {
        var id = Guid.NewGuid().ToString(); // 生成唯一ID
        _sockets.TryAdd(id, socket);
        return id;
    }

    /// <summary>
    /// 移除一个 WebSocket 连接并关闭
    /// </summary>
    public async Task RemoveSocketAsync(string id)
    {
        if (_sockets.TryRemove(id, out var socket))
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
            socket.Dispose();
        }
    }

    /// <summary>
    /// 获取所有连接
    /// </summary>
    public IEnumerable<WebSocket> GetAllSockets() => _sockets.Values;
}