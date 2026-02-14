using System;
using System.Collections.Generic;
using System.Text;

namespace ShopServiceApi.Application.DTOs
{
    public class RegisterUserDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string Role { get; set; } = "Customer"; // پیشفرض Customer
    }
}
