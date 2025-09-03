using IpcServer.Domain.Entities;

namespace IpcServer.WorkFlow.Workflows;

public interface IRecipeExecutor
{
    public void ExecuteRecipe(List<StationRecipe> stations);
}