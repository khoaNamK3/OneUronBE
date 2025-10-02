﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.TokenRepo;
using OneUron.DAL.Repository.UserRepo;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _accessTokenExpirationMinutes;
        private readonly int _refreshTokenExpirationDays;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;

        public JwtService(IConfiguration configuration, ITokenRepository tokenRepository, IUserRepository userRepository)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _secretKey = _configuration["Jwt:SecretKey"];
            _issuer = _configuration["Jwt:Issuer"];
            _audience = _configuration["Jwt:Audience"];
            _accessTokenExpirationMinutes = int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"] ?? "15");
            _refreshTokenExpirationDays = int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"] ?? "7");
        }

        public async Task<string> GenerateAccessTokenWithRolesAsync(Guid userId)
        {
            // Get user with roles
            var user = await _userRepository.GetByIdWithRolesAsync(userId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {userId} not found", nameof(userId));
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add role claims if available
            if (user.Roles != null && user.Roles.Any())
            {
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // For backward compatibility
        public string GenerateAccessToken(Guid userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<(string AccessToken, string RefreshToken)> SaveTokensAsync(Guid userId)
        {
            // Use the new method that includes roles
            string accessToken = await GenerateAccessTokenWithRolesAsync(userId);
            string refreshToken = GenerateRefreshToken();

            // Check if user already has tokens
            var existingToken = await _tokenRepository.GetByUserIdAsync(userId);
            if (existingToken != null)
            {
                // Update existing tokens
                existingToken.RefreshToken = refreshToken;
                await _tokenRepository.UpdateAsync(existingToken);
            }
            else
            {
                // Create new token entry
                var tokenEntity = new Token
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    RefreshToken = refreshToken
                };
                await _tokenRepository.AddAsync(tokenEntity);
            }

            return (accessToken, refreshToken);
        }

        public async Task<(bool Success, string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken)
        {
            // Find token in database
            var tokenEntity = await _tokenRepository.GetByRefreshTokenAsync(refreshToken);
            if (tokenEntity == null)
            {
                return (false, null, null);
            }

            // Generate new tokens with roles
            var newAccessToken = await GenerateAccessTokenWithRolesAsync(tokenEntity.UserId);
            var newRefreshToken = GenerateRefreshToken();

            // Update tokens in database
            tokenEntity.RefreshToken = newRefreshToken;
            await _tokenRepository.UpdateAsync(tokenEntity);

            return (true, newAccessToken, newRefreshToken);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                ValidateLifetime = false // Allow expired tokens
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public bool ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                    ClockSkew = TimeSpan.Zero
                }, out _);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task RevokeRefreshTokenAsync(Guid userId)
        {
            var token = await _tokenRepository.GetByUserIdAsync(userId);
            if (token != null)
            {
                await _tokenRepository.DeleteAsync(token);
            }
        }
    }
}
