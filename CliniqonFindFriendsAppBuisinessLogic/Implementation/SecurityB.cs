using CliniqonFindFriendsAppBussinessLogic.Interfaces;
using CliniqonFindFriendsAppDataAccess.Entities;
using CliniqonFindFriendsAppDataAccess.IRepository;
using CliniqonFindFriendsAppDataAccess.Models;
using CliniqonFindFriendsAppDataAccess.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CliniqonFindFriendsAppBussinessLogic.Implementation
{

    public class SecurityB : ISecurityB
    {
        ISecurityRepository _securityRepository;
        readonly IConfiguration _configuration;
        public SecurityB(ISecurityRepository securityRepository, IConfiguration configuration)
        {
            _securityRepository = securityRepository;
            _configuration = configuration;
        }
        public async Task<string> GenerateJWTToken(string userUnique, int userId, string userAgent)
        {


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSignInKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Issuer","CliniqonFindFriends"),
                new Claim("Admin","true"),
                new Claim(JwtRegisteredClaimNames.UniqueName,userUnique),
                new Claim("UserId",userId.ToString()),
                new Claim("UserAgent",userAgent)
            };
            var token = new JwtSecurityToken("CliniqonFindFriends",
                "CliniqonFindFriends",
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public RefreshTokenModel GenerateRefreshToken(string ipAddress)
        {
            var refreshToken = new RefreshTokenModel
            {
                RefreshToken = GetUniqueToken(),
                // token is valid for 7 days
                Expires = DateTimeOffset.UtcNow.AddDays(7).ToUnixTimeSeconds(),
                CreatedOn = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                CreatedByIP = ipAddress,
            };

            return refreshToken;

        }
        private string GetUniqueToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public async Task<string> SaveRefreshToken(UserProfileModel userProfileModel, string ipAddress, string userAgent)
        {
            RefreshTokenModel jwtRefreshToken = GenerateRefreshToken(ipAddress);
            jwtRefreshToken.UserId = userProfileModel.Id;
            jwtRefreshToken.Browser=userAgent;
            string result = await _securityRepository.SaveRefreshToken(jwtRefreshToken);

            if (result == GeneralDTO.SuccessMessage)
            {
                return jwtRefreshToken.RefreshToken;
            }
            else
            {
                return GeneralDTO.FailedMessage;
            }

        }

        public string ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWTSignInKey"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidAudience = "CliniqonFindFriends",
                    ValidIssuer = "CliniqonFindFriends",
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero

                }, out SecurityToken validatedToken);

                // Corrected access to the validatedToken
                var jwtToken = (JwtSecurityToken)validatedToken;
                var jku = jwtToken.Claims.First(claim => claim.Type == "UserId").Value;
                var userName = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.UniqueName).Value;

                return userName;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> GetActiveStatusOfToken(string token)
        {
            return await _securityRepository.GetActiveStatusOfToken(token);
        }
        public async Task<string> DeleteAllRefreshTokenOfUser(int userId)
        {

            return await _securityRepository.DeleteAllRefreshTokenOfUser(userId);
        }

        public async Task<RefreshTokenModel> GetRefreshToken(string refreshToken)
        {

            return await _securityRepository.GetRefreshToken(refreshToken);
        }

    }
}
