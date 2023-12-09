using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared
{
    public class UserJwtToken
    {
        public JWTToken AccessToken { get; set; }
        public JWTToken RefreshToken { get; set; }

    }

    public class JWTToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
