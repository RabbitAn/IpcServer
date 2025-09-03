using IpcServer.Domain.Entities;

namespace IpcServer.WorkFlow.Devices;

public interface IDeviceAction
{
    /// <summary>
    /// 执行设备动作
    /// </summary>
    /// <param name="station">配方工位信息</param>
    /// <returns>执行结果（OK/NG/PASS/FAIL等）</returns>
    string Execute(StationRecipe station);
}