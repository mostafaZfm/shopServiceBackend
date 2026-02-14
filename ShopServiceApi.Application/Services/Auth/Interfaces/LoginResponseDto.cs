namespace ShopServiceApi.Application.Services.Auth.Interfaces
{
    public class LoginResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }

        public static LoginResponseDto Success(string token)
        {
            return new LoginResponseDto
            {
                IsSuccess = true,
                Token = token
            };
        }

        public static LoginResponseDto Fail(string message)
        {
            return new LoginResponseDto
            {
                IsSuccess = false,
                Message = message
            };
        }
    }
}