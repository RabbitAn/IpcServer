using IpcServer.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace IpcServer.WorkFlow.Devices;

public class ScanAction : IDeviceAction
{
    private readonly ILogger<ScanAction> _logger;

    public ScanAction(ILogger<ScanAction> logger)
    {
        _logger = logger;
    }

    public string Execute(StationRecipe station)
    {
        try
        {
            _logger.LogInformation($"[扫码] 工位:开始执行扫码逻辑 {station.StationName}, 条码规则: {station.BarcodeRule}");
            //模拟校验
            var result = "OK";
            _logger.LogInformation($"[扫码] 工位:校验结果{result}");
            return result;
        }
        catch (Exception e)
        {
            _logger.LogInformation($"[扫码] 工位:出现异常，异常详情：{e.Message}");
            return "NG";
        }
    }
}