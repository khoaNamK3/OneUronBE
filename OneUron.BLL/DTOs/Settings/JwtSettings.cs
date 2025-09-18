using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.Settings
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public int AccessTokenExpirationMinutes { get; set; } = 30;
        public int RefreshTokenExpirationDays { get; set; } = 7;

        public TimeSpan AccessTokenExpiration => TimeSpan.FromMinutes(AccessTokenExpirationMinutes);
        public TimeSpan RefreshTokenExpiration => TimeSpan.FromDays(RefreshTokenExpirationDays);
    }
}
