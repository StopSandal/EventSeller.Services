using AutoMapper;
using DataLayer.Model;
using EventSeller.DataLayer.Entities;
using EventSeller.DataLayer.EntitiesDto.User;
using EventSeller.DataLayer.EntitiesViewModel;
using EventSeller.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.Services.Service
{
    public interface IUserService : IUserActions, IRoleActions
    {
    }
    public interface IUserActions
    {
        Task CreateUser(AddUserDto addUserDto);
        Task<User> GetUserByUserName(string userName);
        Task Update(string id, EditUserDto model);
        Task Delete(string id);
        Task<string> Login(LoginUserVM user);
    }
    public interface IRoleActions
    {
        Task SetRole(string id, string role);
        Task RemoveRole(string id, string role);
    }
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IJWTFactory _JWTFactory;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IJWTFactory jWTFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _JWTFactory = jWTFactory;
        }

        public async Task CreateUser(AddUserDto addUserDto)
        {
            if(await GetUserByUserName(addUserDto.UserName) != null)
            {
                throw new InvalidOperationException("This UserName is already taken.");
            }
            if (await _userManager.FindByEmailAsync(addUserDto.Email) != null)
            {
                throw new InvalidOperationException("This Email is already in use.");
            }
            var user = new User
            {
                UserName = addUserDto.UserName,
                Email = addUserDto.Email
            };

            var result = await _userManager.CreateAsync(user,addUserDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Basic");
            }
        }

        public async Task Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null) 
            {
                throw new InvalidOperationException("User doesn't exists");
            }
            await _userManager.DeleteAsync(user);
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<string> Login(LoginUserVM user)
        {
            var existingUser = await GetUserByUserName(user.UserName);
            if ( existingUser == null)
            {
                existingUser = await _userManager.FindByEmailAsync(user.Email);
            }
            if (existingUser == null)
            {
                throw new InvalidDataException("This user doesn't exists");
            }
            var token = _JWTFactory.GenerateToken(existingUser);
            await _userManager.SetAuthenticationTokenAsync(existingUser, "Server", "jwt", token);
           return _JWTFactory.GenerateToken(existingUser);
        }

        public async Task RemoveRole(string id,string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null) 
            {
                throw new InvalidDataException("This user doesn't exists");
            }
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(result.Errors.ToString());
            }
        }

        public async Task SetRole(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new InvalidDataException("This user doesn't exists");
            }
            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(result.Errors.ToString());
            }
        }

        public async Task Update(string id, EditUserDto model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new NullReferenceException("No User to update");
            _mapper.Map(model, user);
            await _userManager.UpdateAsync(user);
        }
    }
}
