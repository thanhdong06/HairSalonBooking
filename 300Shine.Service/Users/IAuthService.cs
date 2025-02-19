﻿using _300Shine.BusinessObject.DTO.Request;
using _300Shine.DataAccessLayer.DTO.RequestModel;
using _300Shine.DataAccessLayer.DTO.ResponseModel;
using _300Shine.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _300Shine.Service.Users
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(RegisterRequest registerRequest);
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<string> VerifyOtpAsync(VerifyOtpRequest request);

    }
}
