using IpcServer.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace IpcServer.WorkFlow.Devices;

public class LeakTestAction : IDeviceAction
{
    private readonly ILogger<LeakTestAction> _logger;

    
    public LeakTestAction(ILogger<LeakTestAction> logger)
    {
        _logger = logger;
    }


    public string Execute(StationRecipe station)
    {
        try
        {
            _logger.LogInformation($"[气密检测] 工位:开始执行扫码逻辑 {station.StationName}, 条码规则: {station.BarcodeRule}");
            //模拟校验
            var result = "OK";
            _logger.LogInformation($"[气密检测] 工位:校验结果{result}");
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError($"[气密检测] 工位:出现异常，异常详情：{e.Message}");
            return "NG";
        }
    }
}