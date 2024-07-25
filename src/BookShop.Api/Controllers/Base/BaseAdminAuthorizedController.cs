using BookShop.Api.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers.Base;

[Authorize(AuthenticationSchemes = AuthSchemas.AdminFlow)]
[ApiController]
public class BaseAdminAuthorizedController : BaseShopController
{
}
