using IpcServer.Domain.Entities;
using IpcServer.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IpcServer.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class IndexController : ControllerBase
{
    private readonly IRepository<StationRecipe> _repository;

    public IndexController(IRepository<StationRecipe> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var result = await _repository.FindAsync(x => x.StationCode == "OP010");
        var jsonRes = JsonConvert.SerializeObject(result);
        return Ok(jsonRes);
    }
}