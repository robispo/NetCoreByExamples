using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace UserWebApi.Services
{
    public interface IJwtService
    {
        void GenerateToken(HttpContext context, IEnumerable<Claim> claims);
        void RulesTokenValidation(JwtBearerOptions jwtBearerOptions);
        void ValidateAndRenewToken(HttpContext context);
    }
    public class JwtService : IJwtService
    {
        SymmetricSecurityKey _key;
        SigningCredentials _creds;
        JwtSecurityTokenHandler _tokenHandler;
        TokenValidationParameters _tokenValidationParameters;
        IConfiguration _configuration;
        readonly string _baererph, _tokenName;
        readonly IEnumerable<string> _registeredClaimUse;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["securityKey"]));
            _creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            _tokenHandler = new JwtSecurityTokenHandler();
            _baererph = "Bearer ";
            _tokenName = "Authorization";
            _registeredClaimUse = new string[] { "iss", "exp", "aud" };

            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["domain"],
                ValidAudience = _configuration["domain"],
                IssuerSigningKey = _key,
                SaveSigninToken = true,
                ClockSkew = new TimeSpan(0, 0, 10)
            };
        }

        public void GenerateToken(HttpContext context, IEnumerable<Claim> claims)
        {
            JwtSecurityToken token;
            string tokenValue;

            token = new JwtSecurityToken
            (
                issuer : _configuration["domain"],
                audience : _configuration["domain"],
                claims : claims,
                expires : DateTime.Now.AddMinutes(30),
                signingCredentials : _creds
            );

            tokenValue = _tokenHandler.WriteToken(token);
            context.Response.Headers.Add(_tokenName, string.Concat(_baererph, tokenValue));
        }

        public void RulesTokenValidation(JwtBearerOptions options)
        {
            options.TokenValidationParameters = _tokenValidationParameters;
        }

        public void ValidateAndRenewToken(HttpContext context)
        {
            string auth;
            SecurityToken validatedToken;
            ClaimsPrincipal claimsPrincipal;
            IEnumerable<Claim> claims;

            auth = context.Request.Headers[_tokenName];

            if (!string.IsNullOrWhiteSpace(auth))
            {
                auth = auth.Replace(_baererph, string.Empty);

                if (_tokenHandler.CanReadToken(auth))
                {
                    try
                    {
                        claimsPrincipal = _tokenHandler.ValidateToken(auth, _tokenValidationParameters, out validatedToken);
                        claims = claimsPrincipal.Claims.Where(c => !_registeredClaimUse.Contains(c.Type));
                        this.GenerateToken(context, claims);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}