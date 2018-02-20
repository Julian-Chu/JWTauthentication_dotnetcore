using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;

namespace JWTauthentication_dotnetcore.Models
{

  public class TokenManager
  {
    private static string Secret = "TestJWTtestJWTtestJWTtestJWT";
    public static string GenerateToken(string username)
    {
      byte[] key = Convert.FromBase64String(Secret); // Generate Secret String in C#:  HMACSHA256 hmac = new HMACSHA256(); string key = Convert.ToBase64String(hmac.Key);
      SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
      SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
      {
              Subject = new ClaimsIdentity(
                      new[]
                      {
                              new Claim(ClaimTypes.Name, username),
                      }),
              Expires = DateTime.Now.AddMinutes(5),
              SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
      };
      JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
      JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
      //if add custom data:  token.Payload["favouriteFood"] = "cheese";
      return handler.WriteToken(token);
    }

    public static ClaimsPrincipal GetPrincipal(string token)
    {
      try
      {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtToken = (JwtSecurityToken) tokenHandler.ReadToken(token);
        if (jwtToken == null)
          return null;
        byte[] key = Convert.FromBase64String(Secret);
        TokenValidationParameters parameters = new TokenValidationParameters()
        {
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key)
        };
        SecurityToken securityToken;
        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
        return principal;
      }
      catch (Exception exception)
      {
        return null;
      }
    }

    public static string ValidateToken(string token)
    {
      string username = null;
      ClaimsPrincipal princial = GetPrincipal(token);
      if (princial == null)
        return null;
      ClaimsIdentity identity = null;
      try
      {
        identity = (ClaimsIdentity)princial.Identity;
      }
      catch (NullReferenceException)
      {
        return null;
      }

      Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
      username = usernameClaim.Value;
      return username;
    }

  }


}
