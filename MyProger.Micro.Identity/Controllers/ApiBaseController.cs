using MyProger.Micro.Identity.Models.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MyProger.Micro.Identity.Controllers;

[ApiController]
public class ApiBaseController : ControllerBase
{
    protected UserInfo CurrentUser => new(HttpContext.User);
}