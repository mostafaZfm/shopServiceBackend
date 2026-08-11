using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Infrastructure.Identity.Auth
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime Expiration { get; set; }
        public bool IsRevoked { get; set; }
    }
}
