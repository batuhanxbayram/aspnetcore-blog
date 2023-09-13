using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Entity.DTOs.User;
using Blog.Entity.Entities;

namespace Blog.Service.AutoMapper.Users
{
	public class UserProfile:Profile
	{
		public UserProfile()
		{
			CreateMap<UserDTO,AppUser>().ReverseMap();
			CreateMap<UserAddDTO,AppUser>().ReverseMap();
		}
	}
}
