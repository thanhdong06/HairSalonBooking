﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace _300Shine.Configuration
{
    public static class Authentication
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }
                ).AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                        (configuration.GetValue<string>("SECRET_KEY") ?? (configuration["Jwt:secret"]))),
                        ClockSkew = TimeSpan.FromMinutes(5),
                        ValidIssuer = configuration.GetValue<string>("SECRET_ISSUER") ?? configuration["Jwt:issuer"],
                        ValidAudience = configuration.GetValue<string>("SECRET_AUDIENCE") ?? configuration["Jwt:audience"]
                    };
                });

            services.AddAuthorization();
        }
    }
}
