﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace UserWebApi.Services
{
    public interface IJwtService
    {
        void GenerateToken(HttpContext context, IEnumerable<Claim> claims);
        void RulesTokenValidation(JwtBearerOptions jwtBearerOptions);
    }
    public class JwtService : IJwtService
    {
        SymmetricSecurityKey _key;
        SigningCredentials _creds;
        JwtSecurityTokenHandler _tokenHandler;
        TokenValidationParameters _tokenValidationParameters;
        IConfiguration _configuration;
        readonly string _baererph, _tokenName, _securityKey, _domain;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            _securityKey = _configuration["securityKey"];
            _domain = _configuration["domain"];
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey));
            _creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            _tokenHandler = new JwtSecurityTokenHandler();
            _baererph = "Bearer ";
            _tokenName = "Authorization";

            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _domain,
                ValidAudience = _domain,
                IssuerSigningKey = _key
            };
        }

        public void GenerateToken(HttpContext context, IEnumerable<Claim> claims)
        {
            JwtSecurityToken token;
            string tokenValue;

            token = new JwtSecurityToken
            (
                issuer : _domain,
                audience : _domain,
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

     
    }
}