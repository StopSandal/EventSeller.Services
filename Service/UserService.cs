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
        Task<TokenVM> Login(LoginUserVM user);
        Task<TokenVM> RefreshToken(TokenVM token);
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

        public async Task<TokenVM> Login(LoginUserVM user)
        {
            
            var existingUser = await GetUserByUserName(user.UserName) ?? await _userManager.FindByEmailAsync(user.Email);

            if (existingUser == null)
            {
                throw new InvalidDataException("This user doesn't exists");
            }
            // get claims and JWT token
            var claims = await GetUserClaimsAsync(existingUser);
            var token = _JWTFactory.GenerateToken(claims);
            //set JWT token
            await _userManager.RemoveAuthenticationTokenAsync(existingUser, "Server", "jwt");
            await _userManager.SetAuthenticationTokenAsync(existingUser, "Server", "jwt", token);
            //set Refresh token for user
            var refreshToken = _JWTFactory.GenerateRefreshToken();
            existingUser.RefreshToken = refreshToken;
            existingUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(_JWTFactory.GetRefreshTokenValidityInDays());
            await _userManager.UpdateAsync(existingUser);

            return new TokenVM()
            {
                AccessToken = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<TokenVM> RefreshToken(TokenVM token)
        {
            string? accessToken = token.AccessToken;
            string? refreshToken = token.RefreshToken;

            var identity = await _JWTFactory.GetIdentityFromExpiredTokenAsync(accessToken);

            if (identity == null)
            {
                throw new InvalidOperationException("Invalid access token");
            }

            string username = identity.Name;

            var user = await GetUserByUserName(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new InvalidOperationException("Invalid access token or refresh token");
            }


            var newRefreshToken = _JWTFactory.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            var newAccessToken = _JWTFactory.GenerateToken(await GetUserClaimsAsync(user));
            await _userManager.RemoveAuthenticationTokenAsync(user, "Server", "jwt");
            await _userManager.SetAuthenticationTokenAsync(user, "Server", "jwt", newAccessToken);


            return new TokenVM
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
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
        private async Task<IEnumerable<Claim>> GetUserClaimsAsync(User user)
        {
            
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            return claims;
        }
    }
}
