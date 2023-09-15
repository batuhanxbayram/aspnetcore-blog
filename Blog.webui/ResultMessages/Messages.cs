using static Blog.Webui.ResultMessages.Messages;

namespace Blog.Webui.ResultMessages
{
	public static class Messages
	{
		public static class Article
		{
			public static string Add(string articleName)
			{
				return $"{articleName} isimli makale başarıyla eklenmiştir";
			}
			public static string Update(string articleName)
			{
				return $"{articleName} isimli makale başarıyla güncellenmiştir";
			}

		}

		public static class Category
		{
			public static string Add(string categoryName)
			{
				return $"{categoryName} isimli kategori başarıyla eklenmiştir";
			}
			public static string Update(string categoryName)
			{
				return $"{categoryName} isimli kategori başarıyla güncellenmiştir";
			}

		}

		public static class User
		{

			public static string Add(string user)
			{
				return $"{user} isimli kullanıcı başarıyla eklenmiştir";
			}
			public static string Update(string user)
			{
				return $"{user} isimli kullanıcı başarıyla güncellenmiştir";
			}
			public static string Delete(string user)
			{
				return $"{user} isimli kullanıcı başarıyla silinmiştir";
			}
		}
	}
}
