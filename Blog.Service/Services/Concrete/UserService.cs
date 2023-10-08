using System.Security.Claims;
using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.DTOs.User;
using Blog.Entity.Entities;
using Blog.Service.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Service.Services.Concrete
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly IMapper _mapper;
		private readonly UserManager<AppUser> _userManager;
		private readonly ClaimsPrincipal _user;


		public UserService(IUnitOfWork unitOfWork,IHttpContextAccessor httpContextAccessor ,RoleManager<AppRole> roleManager, IMapper mapper, UserManager<AppUser> userManager)
		{
			_unitOfWork = unitOfWork;
			_user = httpContextAccessor.HttpContext.User;
			_roleManager = roleManager;
			_mapper = mapper;
			_userManager = userManager;
		}
		public async Task<List<UserDTO>> GetAllUserWithRoleAsync()
		{
			var users = await _userManager.Users.ToListAsync();
			var map = _mapper.Map<List<UserDTO>>(users);

			foreach (var item in map)
			{
				var findUser = await _userManager.FindByIdAsync(item.Id.ToString());
				var role = string.Join("", await _userManager.GetRolesAsync(findUser));
				item.Role = role;
			}

			return map;
		}

		public async Task<List<AppRole>> GetAllRolesAsync()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			return roles;
		}

		public async Task<IdentityResult> CreateUserAsync(UserAddDTO userAddDto)
		{
			var map = _mapper.Map<AppUser>(userAddDto);
			map.UserName = userAddDto.Email;
			var result = await _userManager.CreateAsync(map, string.IsNullOrEmpty(userAddDto.Password) ? "" : userAddDto.Password);
			if (result.Succeeded)
			{
				var findRole = await _roleManager.FindByIdAsync(userAddDto.RoleId.ToString());
				await _userManager.AddToRoleAsync(map, findRole.ToString());
				return result;
			}
			else
				return result;
		}


		public async Task<AppUser> GetUserByIdAsync(Guid userId)
		{
			return await _userManager.FindByIdAsync(userId.ToString());
		}

		public async Task<string> GetUserRoleAsync(AppUser user)
		{
			return string.Join("", await _userManager.GetRolesAsync(user));
		}

		public async Task<(IdentityResult,string)> DeleteUserAsync(Guid userId)
		{
			var user = await GetUserByIdAsync(userId);
			var result = await _userManager.DeleteAsync(user);
			return (result,user.Email);
		}

		public async Task<UserProfileDTO> GetUserProfileAsync()
		{
			
		}
	}
}
