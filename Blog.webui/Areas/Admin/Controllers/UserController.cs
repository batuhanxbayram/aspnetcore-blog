using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.DTOs.User;
using Blog.Entity.Entities;
using Blog.Webui.ResultMessages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Data;

namespace Blog.Webui.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class UserController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly RoleManager<AppRole> _roleManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly IToastNotification _toastNotification;
		private readonly IUnitOfWork _unitOfWork;


		public UserController(IMapper mapper, IHttpContextAccessor httpContextAccessor,
			RoleManager<AppRole> roleManager, UserManager<AppUser> userManager,
			IToastNotification toastNotification, IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_roleManager = roleManager;
			_userManager = userManager;
			_toastNotification = toastNotification;
			_unitOfWork = unitOfWork;
		}

		public async Task<IActionResult> Index()
		{
			var users = await _userManager.Users.ToListAsync();
			var map = _mapper.Map<List<UserDTO>>(users);

			foreach (var item in map)
			{
				var findUser = await _userManager.FindByIdAsync(item.Id.ToString());
				var role = string.Join("", await _userManager.GetRolesAsync(findUser));
				item.Role = role;
			}

			return View(map);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			var roles = await _roleManager.Roles.ToListAsync();

			return View(new UserAddDTO()
			{
				Roles = roles,
			});
		}



		[HttpPost]
		public async Task<IActionResult> Add(UserAddDTO userAddDTO)
		{
			var roles = await _roleManager.Roles.ToListAsync();

			var map = _mapper.Map<AppUser>(userAddDTO);

			if (ModelState.IsValid)
			{
				map.UserName = userAddDTO.Email;
				var result = await _userManager.CreateAsync(map, userAddDTO.Password);
				if (result.Succeeded)
				{
					var findRole = await _roleManager.FindByIdAsync(userAddDTO.RoleId.ToString());
					await _userManager.AddToRoleAsync(map, findRole.ToString());
					_toastNotification.AddSuccessToastMessage(Messages.User.Add(userAddDTO.Email),
						new ToastrOptions() { Title = "İşlem Başarılı" });
					return RedirectToAction("Index", "User", new { Area = "Admin" });
				}
				else
				{
					foreach (var item in result.Errors)
					{
						ModelState.AddModelError("", item.Description);
					}

					return View(new UserAddDTO()
					{
						Roles = roles,
					});
				}

			}
			else
			{
				return View(userAddDTO);
			}

		}

		[HttpGet]
		public async Task<IActionResult> Update(Guid userId)
		{
			var user = await _userManager.FindByIdAsync(userId.ToString());
			var roles = await _roleManager.Roles.ToListAsync();
			var map = _mapper.Map<UserUpdateDTO>(user);
			map.Roles = roles;
			return View(map);
		}

		[HttpPost]
		public async Task<IActionResult> Update(UserUpdateDTO userUpdateDTO)
		{
			var user = await _userManager.FindByIdAsync(userUpdateDTO.Id.ToString());

			if (user != null)
			{
				var userRole = string.Join("", await _userManager.GetRolesAsync(user));
				var roles = await _roleManager.Roles.ToListAsync();
				if (ModelState.IsValid)
				{
					user.FirstName = userUpdateDTO.FirstName;
					user.LastName = userUpdateDTO.LastName;
					user.Email = userUpdateDTO.Email;
					user.PhoneNumber = userUpdateDTO.PhoneNumber;
					user.UserName = userUpdateDTO.Email;
					user.SecurityStamp = Guid.NewGuid().ToString();
					var result = await _userManager.UpdateAsync(user);
					if (result.Succeeded)
					{
						await _userManager.RemoveFromRoleAsync(user, userRole);
						var findrole = await _roleManager.FindByIdAsync(userUpdateDTO.RoleId.ToString());
						await _userManager.AddToRoleAsync(user, findrole.Name);
						_toastNotification.AddSuccessToastMessage(Messages.User.Update(userUpdateDTO.Email),
							new ToastrOptions() { Title = "İşlem Başarılı" });
						return RedirectToAction("Index", "User", new { Area = "Admin" });
					}
					else
					{
						foreach (var item in result.Errors)
						{
							ModelState.AddModelError("", item.Description);
							return View(new UserUpdateDTO() { Roles = roles });
						}
					}
				}
			}

			return View();
		}


		public async Task<IActionResult> Delete(Guid userId)
		{
			var user = await _userManager.FindByIdAsync(userId.ToString());

			var result = await _userManager.DeleteAsync(user);

			if (result.Succeeded)
			{
				_toastNotification.AddSuccessToastMessage(Messages.User.Delete(user.Email), new ToastrOptions() { Title = "İşlem Başarılı" });
				return RedirectToAction("Index", "User", new { Area = "Admin" });
			}
			else
			{
				foreach (var item in result.Errors)
				{
					ModelState.AddModelError("", item.Description);
				}
			}
			return NotFound();
		}

		
	}
}
