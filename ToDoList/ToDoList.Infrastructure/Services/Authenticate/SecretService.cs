using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Domain.JWT;
using ToDoList.Domain.Domain.Users;
using ToDoList.Domain.Domain.Users.Response;
using ToDoList.Infrastructure.DbAccess;

namespace ToDoList.Infrastructure.Services.Authenticate
{
    public class SecretService : ISecretService
    {
        private readonly JwtSettings _settings;
        private readonly AppDbContext _dbContext;
        private readonly TokenValidationParameters _tokenValidationParameters;

        private const string _SECURITY_ALGORITHM = SecurityAlgorithms.HmacSha256Signature;

        public SecretService(JwtSettings settings, AppDbContext dbContext, TokenValidationParameters tokenValidationParameters)
        {
            _settings = settings;
            _dbContext = dbContext;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<AuthenticatedUserResponse> GenerateJWT(User user, string refreshToken = "")
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(_settings.Expires),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_settings.IssuerSigningKey)),
                        _SECURITY_ALGORITHM),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                    new Claim("id", user.Id.ToString())
                })
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            if(string.IsNullOrEmpty(refreshToken))
                return await GenerateRefreshToken(new AuthenticatedUserResponse
                {
                    Token = tokenHandler.WriteToken(token),
                    Success = true,
                }, user);
            else
            {
                return new AuthenticatedUserResponse
                {
                    Token = tokenHandler.WriteToken(token),
                    RefreshToken = refreshToken,
                    Success = true
                };
            }
        }

        public async Task<AuthenticatedUserResponse> GenerateJWTFromRefreshToken(string token, string refreshToken)
        {
            var validatedToken = GetClaimsPrincipalFromToken(token);

            if (validatedToken == null)
                return new AuthenticatedUserResponse
                {
                    Errors = new[] { "Invalid Token" }
                };

            // Get seconds from the validatedToken Claims
            var expiryDateUnix = long.Parse(validatedToken.Claims
                .Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix)
                .AddMinutes(-1); // Clock Skew

            // Check if the expiry date is in future
            if (expiryDateTimeUtc >= DateTime.UtcNow)
                return new AuthenticatedUserResponse
                {
                    Errors = new[] { "This token hasn't expired yet" }
                };

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _dbContext.RefreshTokens
                .SingleOrDefaultAsync(x => x.RToken == refreshToken);

            if (storedRefreshToken == null)
                return new AuthenticatedUserResponse
                {
                    Errors = new[] { "This refresh token does not exist" }
                };

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                return new AuthenticatedUserResponse
                {
                    Errors = new[] { "This refresh token has already expired" }
                };

            if (storedRefreshToken.Invalidated)
                return new AuthenticatedUserResponse
                {
                    Errors = new[] { "This refresh token has been invalidated" }
                };

            if (storedRefreshToken.Used)
                return new AuthenticatedUserResponse
                {
                    Errors = new[] { "This refresh token has been used" }
                };

            if (storedRefreshToken.JwtId != jti)
                return new AuthenticatedUserResponse
                {
                    Errors = new[] { "This refresh token does not match this JWT" }
                };

            storedRefreshToken.Used = true;
            _dbContext.RefreshTokens.Update(storedRefreshToken);
            await _dbContext.SaveChangesAsync();

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == storedRefreshToken.UserId);

            // Generate new refresh token - emtpy refresh token
            return await GenerateJWT(user, "");
        }

        public string GeneratePasswordHash(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            byte[] salt = Encoding.UTF8.GetBytes(_settings.IssuerSigningKey);

            // derive a 256-bit subkey (use HMACSHA256 with 10,000 iterations)
            // 256 bits = 32 bytes
            string hashedPass = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password,
                    salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    10_000,
                    numBytesRequested: 32));

            return hashedPass;
        }

        private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Prevent from checking already expired token...
                _tokenValidationParameters.ValidateLifetime = false;
                // Does not check if security algorithm is the same
                var principal = tokenHandler.ValidateToken(token,
                    _tokenValidationParameters, out var validatedtoken);

                _tokenValidationParameters.ValidateLifetime = false;

                // So we are checking it
                return IsJwtValidSecurityAlgorithm(validatedtoken) ? principal : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static bool IsJwtValidSecurityAlgorithm(SecurityToken token)
        {
            return (token is JwtSecurityToken jwtToken) &&
                (jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<AuthenticatedUserResponse> GenerateRefreshToken(AuthenticatedUserResponse response, User user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(_settings.RefreshTokenExpires),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_settings.IssuerSigningKey)),
                        _SECURITY_ALGORITHM),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                    new Claim("id", user.Id.ToString())
                })
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
            };

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            return new AuthenticatedUserResponse
            {
                Token = response.Token,
                Success = true,
                // Token is autogenerated
                RefreshToken = refreshToken.RToken
            };
        }
    }
}
