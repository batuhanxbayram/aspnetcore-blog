using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Entity.DTOs.User;
using Blog.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace Blog.Service.Services.Abstract
{
	public interface IUserService
	{
		Task<List<UserDTO>> GetAllUserWithRoleAsync();
		Task<List<AppRole>> GetAllRolesAsync();
		Task<IdentityResult> CreateUserAsync(UserAddDTO userAddDto);
		// Task<IdentityResult> UpdateUserAsync(UserUpdateDTO userUpdateDto);
		Task<AppUser> GetUserByIdAsync(Guid userId);
		Task<string> GetUserRoleAsync(AppUser user);
		Task<(IdentityResult identityResult,string email)> DeleteUserAsync(Guid userId);
		Task<UserProfileDTO> GetUserProfileAsync();




	}
}
