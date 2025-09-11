namespace OneUron.BLL.DTOs
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid? UserId { get; set; }
        public string Token { get; set; } // Optional: JWT token
    }
}