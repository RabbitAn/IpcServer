using IpcServer.Domain.Entities;
using IpcServer.WorkFlow.Devices;
using Microsoft.Extensions.Logging;

namespace IpcServer.WorkFlow.Workflows;

public class RecipeExecutor:IRecipeExecutor
{
    private readonly ILogger<RecipeExecutor> _logger;
    private readonly Dictionary<string, IDeviceAction> _actions;


    public RecipeExecutor(ILogger<RecipeExecutor> logger, LeakTestAction leakTestAction, ScanAction scanAction,
        TightenAction tightenAction)
    {
        _logger = logger;
        _actions = new Dictionary<string, IDeviceAction>(StringComparer.OrdinalIgnoreCase)
        {
            { "拧紧", tightenAction },
            { "扫描物料", scanAction },
            { "扫描员工号", scanAction },
            { "气密性测试", leakTestAction },
        };
    }

    public void ExecuteRecipe(List<StationRecipe> stations)
    {
        for (int i = 0; i < stations.Count - 1; i++)
        {
            if (!_actions.ContainsKey(stations[i].OperationType))
            {
                _logger.LogInformation($"未知操作类型: {stations[i].OperationType}，跳过");
                continue;
            }

            //获取操作类型对应的执行器
            var action = _actions[stations[i].OperationType];
            string result = action.Execute(stations[i]);
            _logger.LogInformation($"执行结果: {result}");

            if (stations[i].NeedVerification && !string.Equals(result, "OK", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(result, "PASS", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("结果不符合要求，中断执行");
                break;
            }
        }
    }
}