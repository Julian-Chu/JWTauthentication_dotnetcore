using JWTauthentication_dotnetcore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JWTauthentication_dotnetcore.filter
{
  public class JWTAuthorizationAttribute : ActionFilterAttribute, IAuthorizationFilter
  {
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      var header = context.HttpContext.Request.Headers;
      var token = header["token"];
      var username = header["username"];
      bool exists = new UserRepository().GetUser(username) != null;
      if (!exists)
      {
        context.Result = new UnauthorizedResult();
        return;
      }

      string tokenUsername = TokenManager.ValidateToken(token);
      if (!username.Equals(tokenUsername))
      {
        context.Result = new UnauthorizedResult();
      }
    }
  }
}