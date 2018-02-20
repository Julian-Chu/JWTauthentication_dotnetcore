using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JWTauthentication_dotnetcore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTauthentication_dotnetcore.Controllers
{
    [Produces("application/json")]
    [Route("api/Login")]
    public class LoginController : Controller
    {
      [HttpPost]
      public IActionResult Login([FromBody]User user)
      {
        User u = new UserRepository().GetUser(user.Username);
        if (u == null)
          return new NotFoundObjectResult("The user was not found");
        bool credentials = u.Password.Equals(user.Password);
        if (!credentials) return new ForbidResult("The username/password was wrong");
        return new OkObjectResult(TokenManager.GenerateToken(user.Username));
      }

      [HttpGet]
      public IActionResult Validate([FromHeader]string token, [FromHeader]string username)
      {
        bool exists = new UserRepository().GetUser(username) != null;
        if (!exists) return new NotFoundObjectResult("The user was not found");
        string tokenUsername = TokenManager.ValidateToken(token);
        if (username.Equals(tokenUsername))
          return Ok();
        return BadRequest();
      }
    }
}