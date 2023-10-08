using AutoMapper;
using Blog.Entity.DTOs.User;
using Blog.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

namespace Blog.Webui.Areas.Admin.ViewComponents
{
	public class DashboardHeaderViewComponent:ViewComponent
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IMapper _mapper;

		public DashboardHeaderViewComponent(UserManager<AppUser> userManager,IMapper mapper)
		{
			_userManager = userManager;
			_mapper = mapper;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var userinfo = await _userManager.GetUserAsync(HttpContext.User);

			var map = _mapper.Map<UserDTO>(userinfo);

			var role = string.Join("", await _userManager.GetRolesAsync(userinfo));

			map.Role = role;

			return View(map);
		}

	}
}
