using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet.Controllers;

[ApiController]
public class AdminController : ControllerBase
{
    [HttpGet]
    [Route("admin")]
    [Authorize(Roles = "admin")]
    public IActionResult GetAdmin()
    {
        return Ok("Access granted to administrators");
    }

    [HttpGet]
    [Route("admin/authz")]
    [PermissionAuthorize("admin_resource", "edit")]
    public Task<IActionResult> GetAdminAuthz([FromServices] IHttpClientFactory httpClientFactory)
    {
        return Task.FromResult<IActionResult>(Ok("Access granted by the Authorization Server"));
    }

    [HttpGet]
    [Route("public")]
    [AllowAnonymous]
    public IActionResult GetPublic()
    {
        return Ok("Public Access");
    }

}