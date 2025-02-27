﻿using _300Shine.BusinessObject.DTO.Request;
using _300Shine.DataAccessLayer.DTO.RequestModel;
using _300Shine.DataAccessLayer.DTO.ResponseModel;
using _300Shine.DataAccessLayer.Entities;
using _300Shine.Repository.Repositories.User;
using AutoMapper;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _300Shine.Service.Users
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<string> CreateStylistAsync(CreateUserRequest request)
        {
            return await _userRepository.CreateStylistAsync(request);
        } 
        public async Task<string> CreateManagerAsync(CreateUserRequest request)
        {
            return await _userRepository.CreateManagerAsync(request);
        }

        public async Task<string> DeleteUserAsync(int userId)
        {
            return await _userRepository.DeleteUserAsync(userId);
        }

        public async Task<List<ResponseUser>> GetAllUsersAsync(int? roleId = null)
        {
            return await _userRepository.GetAllUsersAsync(roleId);
        }

        public async Task<ResponseUser> GetUserByPhoneAsync(string phone)
        {
            return await _userRepository.GetUserByPhoneAsync(phone);
        }
        public async Task<ResponseUser> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<string> UpdateUserAsync(int userId, UpdateUserRequest request)
        {
            return await _userRepository.UpdateUserAsync(userId, request);
        }
    }
}
