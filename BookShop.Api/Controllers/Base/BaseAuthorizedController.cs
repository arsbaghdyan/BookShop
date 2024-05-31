using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers.Base;

[Authorize]
[ApiController]
public class BaseAuthorizedController : ControllerBase
{
}
