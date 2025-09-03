using IpcServer.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace IpcServer.WorkFlow.Devices;

public class TightenAction : IDeviceAction
{
    private readonly ILogger<TightenAction> _logger;

    public TightenAction(ILogger<TightenAction> logger)
    {
        _logger = logger;
    }

    public string Execute(StationRecipe station)
    {
        try
        {
            _logger.LogInformation(
                $"[拧紧] 工位: 开始执行拧紧程序，{station.StationName}, 数量: {station.Quantity}, 扭矩范围: {station.TorqueRange}, 角度范围: {station.AngleRange}");
            var res = "OK";
            _logger.LogInformation($"[拧紧] 工位: 结束拧紧程序，拧紧结果{res}");
            return res;
        }
        catch (Exception e)
        {
            _logger.LogInformation($"[拧紧] 工位:出现异常，异常详情：{e.Message}");
            return "NG";
        }
    }
}