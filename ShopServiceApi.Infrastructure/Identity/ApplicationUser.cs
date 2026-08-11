using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;


namespace ShopServiceApi.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Province { get; set; } = "-";
        public string City { get; set; } = "-";
        public string PostalCode { get; set; } = "00000";
    }
}
