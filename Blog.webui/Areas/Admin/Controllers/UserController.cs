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
using Blog.Service.Services.Abstract;
using Blog.Service.Services.Concrete;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using static Blog.Webui.ResultMessages.Messages;

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
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IUserService _userService;
		private readonly IValidator _validator;


		public UserController(IMapper mapper, IHttpContextAccessor httpContextAccessor,
			RoleManager<AppRole> roleManager, UserManager<AppUser> userManager,
			IToastNotification toastNotification, IUnitOfWork unitOfWork, SignInManager<AppUser> signInManager, IUserService userService, IValidator<AppUser> validator)
		{
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_roleManager = roleManager;
			_userManager = userManager;
			_toastNotification = toastNotification;
			_unitOfWork = unitOfWork;
			_signInManager = signInManager;
			_userService = userService;
			_validator = validator;
		}

		public async Task<IActionResult> Index()
		{
			var dto = await _userService.GetAllUserWithRoleAsync();
			return View(dto);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			var roles = await _userService.GetAllRolesAsync();

			return View(new UserAddDTO()
			{
				Roles = roles,
			});
		}



		[HttpPost]
		public async Task<IActionResult> Add(UserAddDTO userAddDto)
		{
			var map = _mapper.Map<AppUser>(userAddDto);

			var roles = await _userService.GetAllRolesAsync();

			if (ModelState.IsValid)
			{
				var result = await _userService.CreateUserAsync(userAddDto);
				if (result.Succeeded)
				{
					_toastNotification.AddSuccessToastMessage(Messages.User.Add(userAddDto.Email), new ToastrOptions { Title = "İşlem Başarılı" });
					return RedirectToAction("Index", "User", new { Area = "Admin" });
				}
				else
				{
					ModelState.AddModelError("", "Hata");
					return View(new UserAddDTO { Roles = roles });
				}
			}
			return View(new UserAddDTO { Roles = roles });
		}

		[HttpGet]
		public async Task<IActionResult> Update(Guid userId)
		{
			var user = await _userService.GetUserByIdAsync(userId);
			var roles = await _userService.GetAllRolesAsync();
			var map = _mapper.Map<UserUpdateDTO>(user);
			map.Roles = roles;
			return View(map);
		}

		[HttpPost]
		public async Task<IActionResult> Update(UserUpdateDTO userUpdateDTO)
		{
			var user = await _userService.GetUserByIdAsync(userUpdateDTO.Id);

			if (user != null)
			{
				var userRole = await _userService.GetUserRoleAsync(user);
				var roles = await _userService.GetAllRolesAsync();
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

			var result = await _userService.DeleteUserAsync(userId);

			if (result.identityResult.Succeeded)
			{
				_toastNotification.AddSuccessToastMessage(Messages.User.Delete(result.email), new ToastrOptions() { Title = "İşlem Başarılı" });
				return RedirectToAction("Index", "User", new { Area = "Admin" });
			}
			else
			{ 
				foreach (var item in result.identityResult.Errors)
				{
					ModelState.AddModelError("", item.Description);
				}
			}
			return NotFound();
		}

		[HttpGet]
		public async Task<IActionResult> Profile()
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);

			var map = _mapper.Map<UserProfileDTO>(user);

			return View(map);
		}

		[HttpPost]
		public async Task<IActionResult> Profile(UserProfileDTO userProfileDto)
		{
			var user = await _userManager.GetUserAsync(HttpContext.User);
			if (ModelState.IsValid)
			{
				if (userProfileDto.CurrentPassword is null)
				{
					_toastNotification.AddErrorToastMessage("Değişiklik yapmak için şifrenizi girmelisiniz");
					return View();
				}
				var isVerified = await _userManager.CheckPasswordAsync(user, userProfileDto.CurrentPassword);
				if (isVerified && userProfileDto.NewPassword != null)
				{
					var result = await _userManager.ChangePasswordAsync(user, userProfileDto.CurrentPassword, userProfileDto.NewPassword);
					if (result.Succeeded)
					{
						await _userManager.UpdateSecurityStampAsync(user);
						await _signInManager.SignOutAsync();
						await _signInManager.PasswordSignInAsync(user, userProfileDto.NewPassword, true, false);

						user.FirstName = userProfileDto.FirstName;
						user.LastName = userProfileDto.LastName;
						user.PhoneNumber = userProfileDto.PhoneNumber;

						await _userManager.UpdateAsync(user);
						_toastNotification.AddSuccessToastMessage("Şifreniz ve bilgileriniz başarıyla güncellenmiştir");
						return View();
					}
					else
					{

						return View();
					}
				}
				else if (isVerified)
				{
					await _userManager.UpdateSecurityStampAsync(user);
					user.FirstName = userProfileDto.FirstName;
					user.LastName = userProfileDto.LastName;
					user.PhoneNumber = userProfileDto.PhoneNumber;

					await _userManager.UpdateAsync(user);
					_toastNotification.AddSuccessToastMessage("Bilgileriniz başarıyla güncellenmiştir");
					return View();
				}
				else
				{
					_toastNotification.AddErrorToastMessage("Bilgileriniz güncellenirken hata oluştu");

				}
			}

			return View();
		}
	}
}
