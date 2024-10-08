using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet.Controllers;

[ApiController]
public class AppController : ControllerBase
{
    [HttpGet]
    [Route("oil")]
    [PermissionAuthorize("Oil Platform - Australia", "edit")]
    public Task<IActionResult> GetOil()
    {
        return Task.FromResult<IActionResult>(
            Ok("Access to Oil Platform - Australia granted by the Authorization Server"));
    }

    [HttpGet]
    [Route("operations")]
    [PermissionAuthorize("Copper Mine Operations - Peru", "edit")]
    public Task<IActionResult> GetOperations([FromServices] IHttpClientFactory httpClientFactory)
    {
        return Task.FromResult<IActionResult>(
            Ok("Access to Cooper Mine Operations granted by the Authorization Server"));
    }

    [HttpGet]
    [Route("public")]
    [AllowAnonymous]
    public IActionResult GetPublic()
    {
        return Ok("Public Access");
    }
}