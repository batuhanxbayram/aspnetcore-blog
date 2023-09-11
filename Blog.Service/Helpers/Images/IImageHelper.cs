using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Entity.DTOs.Images;
using Blog.Entity.Enums;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Helpers.Images
{
	public interface IImageHelper
	{
		Task<ImageUploadedDTO> Upload(string name, IFormFile imageFile, ImageType imageType, string folderName = null);
		void Delete(string imageName);
	}
}
