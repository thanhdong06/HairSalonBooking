﻿using _300Shine.BusinessObject.DTO.Request;
using _300Shine.DataAccessLayer.DTO.RequestModel;
using _300Shine.DataAccessLayer.DTO.ResponseModel;
using _300Shine.DataAccessLayer.Entities;
using _300Shine.Repository;
using _300Shine.Repository.Repositories.User;
using _300Shine.Service.SMS;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Twilio.Jwt.AccessToken;

namespace _300Shine.Service.Users
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthRepository _authRepository;
        private readonly ISMSService _smsService;

        public AuthService(IConfiguration configuration, IAuthRepository authRepository, ISMSService smsService)
        {
            _configuration = configuration;
            _authRepository = authRepository;
            _smsService = smsService;
        }

        public async Task<string> RegisterUserAsync(RegisterRequest registerRequest)
        {
            registerRequest.Phone = FormatPhoneNumber(registerRequest.Phone);
            var result = await _authRepository.Register(registerRequest);

            //var otp = _smsService.GenerateOtp();
            //await _smsService.SendOtpSmsAsync(registerRequest.Phone, otp);

            var user = await _authRepository.GetUserByPhoneAsync(registerRequest.Phone);
            user.Otp = string.Empty;
            await _authRepository.UpdateUserAsync(user);
            return result;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            request.Phone = FormatPhoneNumber(request.Phone);
            var user = await _authRepository.LoginAsync(request);

            if (user == null)
            {
                throw new InvalidDataException("Invalid phone number or password");
            }
            var token = CreateToken(user);

            var responseUser = new ResponseUser
            {
                Id = user.Id,
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Phone = user.Phone,
                Address = user.Address,
                IsVerified = user.IsVerified,
                Status = user.Status,
                SalonId = user.SalonId,
                RoleName = user.Role.Name
            };

            return new LoginResponse
            {
                Token = token,
                User = responseUser
            };
        }

        public async Task<string> VerifyOtpAsync(VerifyOtpRequest request)
        {
            request.Phone = FormatPhoneNumber(request.Phone);
            var user = await _authRepository.GetUserByPhoneAsync(request.Phone);
            if (user == null)
            {
                return "User not found.";
            }

            if (user.Otp == request.Otp)
            {
                user.IsVerified = true;
                user.Otp = string.Empty; // Clear OTP after verification
                await _authRepository.UpdateUserAsync(user);
                return "Phone number verified successfully.";
            }
            else
            {
                return "";
            }
        }

        private string CreateToken(UserEntity user)
        {
            if (user == null || user.Phone.IsNullOrEmpty())
            {
                throw new ArgumentException("Invalid login request", nameof(user));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:secret"]);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Phone.ToString()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:issuer"],
                Audience = _configuration["Jwt:audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string FormatPhoneNumber(string phone)
        {
            if (!string.IsNullOrEmpty(phone) && phone.StartsWith("0"))
            {
                return "+84" + phone.Substring(1);
            }
            return phone;
        }

    }
}
