using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTauthentication_dotnetcore
{
  public class User
  {
    public string Username { get; set; }
    public string Password { get; set; }
  }


  public enum UserRole
  {
    Normal,
    Admin
  }
}
