using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Entity.DTOs.Images;
using Blog.Entity.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Service.Helpers.Images
{
	public class ImageHelper : IImageHelper
	{
		private readonly IWebHostEnvironment _env;
		private readonly string wwwroot;
		private const string imgFolder = "images";
		private const string articleImagesFolder = "article-images";
		private const string userImagesFolder = "user-images";


		public ImageHelper(IWebHostEnvironment env)
		{
			_env = env;
			wwwroot = env.WebRootPath;
		}
		private string ReplaceInvalidChars(string fileName)
		{
			return fileName.Replace("İ", "I")
				.Replace("ı", "i")
				.Replace("Ğ", "G")
				.Replace("ğ", "g")
				.Replace("Ü", "U")
				.Replace("ü", "u")
				.Replace("ş", "s")
				.Replace("Ş", "S")
				.Replace("Ö", "O")
				.Replace("ö", "o")
				.Replace("Ç", "C")
				.Replace("ç", "c")
				.Replace("é", "")
				.Replace("!", "")
				.Replace("'", "")
				.Replace("^", "")
				.Replace("+", "")
				.Replace("%", "")
				.Replace("/", "")
				.Replace("(", "")
				.Replace(")", "")
				.Replace("=", "")
				.Replace("?", "")
				.Replace("_", "")
				.Replace("*", "")
				.Replace("æ", "")
				.Replace("ß", "")
				.Replace("@", "")
				.Replace("€", "")
				.Replace("<", "")
				.Replace(">", "")
				.Replace("#", "")
				.Replace("$", "")
				.Replace("½", "")
				.Replace("{", "")
				.Replace("[", "")
				.Replace("]", "")
				.Replace("}", "")
				.Replace(@"\", "")
				.Replace("|", "")
				.Replace("~", "")
				.Replace("¨", "")
				.Replace(",", "")
				.Replace(";", "")
				.Replace("`", "")
				.Replace(".", "")
				.Replace(":", "")
				.Replace(" ", "");
		}
		public async Task<ImageUploadedDTO> Upload(string name, IFormFile imageFile, ImageType imageType, string folderName = null)
		{
			folderName ??= imageType == ImageType.User ? userImagesFolder : articleImagesFolder;
			if (!Directory.Exists($"{wwwroot}/{imgFolder}/{folderName}"))
			{
				Directory.CreateDirectory($"{wwwroot}/{imgFolder}/{folderName}");
			}

			var olfFileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
			var fileExtension = Path.GetExtension(imageFile.FileName);

			name = ReplaceInvalidChars(name);

			DateTime dateTime= DateTime.Now;
			string newFileName = $"{name}_{dateTime.Millisecond}{fileExtension}";
			string path = Path.Combine($"{wwwroot}/{imgFolder}/{folderName}/{newFileName}");

			await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None,
				1024 * 1024, useAsync: false);
			await imageFile.CopyToAsync(stream);
			await stream.FlushAsync();

			return new ImageUploadedDTO()
			{
				FullName = $"{folderName}/{newFileName}"
			};
		}

		public void Delete(string imageName)
		{
			var filetodelete = Path.Combine($"{wwwroot}/{imgFolder}/{imageName}");

			if(File.Exists(filetodelete))
				File.Delete(filetodelete);
		}
	}
}
