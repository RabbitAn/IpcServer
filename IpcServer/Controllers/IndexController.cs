using IpcServer.Domain.Entities;
using IpcServer.Domain.Interfaces;
using IpcServer.WorkFlow.Workflows;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IpcServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class IndexController : ControllerBase
{
    private readonly IRepository<StationRecipe> _repository;
    private readonly IRecipeExecutor _recipeExecutor;

    public IndexController(IRepository<StationRecipe> repository,IRecipeExecutor recipeExecutor)
    {
        _repository = repository;
        _recipeExecutor = recipeExecutor;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var result = await _repository.FindAsync(x => x.StationCode == "OP010");
       
   
        _recipeExecutor.ExecuteRecipe(result.ToList());
        var res = result.OrderBy(x => x.ProcessStep);
        var jsonRes = JsonConvert.SerializeObject(res);
        return Ok(jsonRes);
    }
}