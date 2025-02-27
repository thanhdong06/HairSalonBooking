﻿using _300Shine.BusinessObject.DTO.Request;
using _300Shine.DataAccessLayer.DBContext;
using _300Shine.DataAccessLayer.DTO.RequestModel;
using _300Shine.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Service.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _300Shine.Repository.Repositories.User
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;

        public AuthRepository(AppDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<string> Register(RegisterRequest request)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Phone == request.Phone && !u.IsDeleted);
            if (existingUser != null)
            {
                throw new InvalidDataException("Phone number already exists");
            }
            var newUser = new UserEntity()
            {
                FullName = request.FullName,
                Password =_passwordService.HashPassword(request.Password),
                DateOfBirth = request.DateOfBirth,
                Gender = request.Gender,
                Phone = request.Phone,
                Address = request.Address,
                RoleId = 3,
                SalonId = 1,
                IsVerified = true,
                Status = "Active",
                ImageUrl = request.ImageUrl,
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return "Register successfully";
        }

        public async Task<UserEntity> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Phone == request.Phone && !u.IsDeleted);

            if (user == null)
            {
                throw new InvalidDataException("Invalid phone number");
            }
            var samePassword = _passwordService.VerifyPassword(request.Password, user.Password);
            if(samePassword) { return user; }
           throw new Exception("Wrong Phone Or Password");
        }

        public async Task<UserEntity> GetUserByPhoneAsync(string phone)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Phone == phone);
        }

        public async Task UpdateUserAsync(UserEntity user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
