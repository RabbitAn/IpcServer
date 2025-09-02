using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IpcServer.Controllers;



[ApiController]
[Route("[controller]/[action]")]
public class LoginController:ControllerBase
{
    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            if (loginRequest.UserName=="admin"&&loginRequest.Password=="123456")
            {
                var response = new 
                {
                    success=true
                };
               return Ok(JsonConvert.SerializeObject(response));
            }
            else
            {
                return BadRequest("登录失败！");
            }
        }
        catch (Exception e)
        {
            return BadRequest("登录失败！");
        }
     
    }
}