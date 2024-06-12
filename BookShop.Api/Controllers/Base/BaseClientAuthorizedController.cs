using BookShop.Api.Constants;
using Microsoft.AspNetCore.Authorization;

namespace BookShop.Api.Controllers.Base;

[Authorize(AuthenticationSchemes = AuthSchemas.ClientFlow)]
public class BaseClientAuthorizedController : BaseShopController
{
}
