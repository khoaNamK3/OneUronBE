using System;
using System.Collections.Generic;

namespace OneUron.BLL.DTOs.AuthDTOs
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}