using static Blog.Webui.ResultMessages.Messages;

namespace Blog.Webui.ResultMessages
{
	public static class Messages
	{
		public static class Article
		{


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
				return $"{user} isimli kullanıcı başarıyla güncellenmiştir";
			}
			public static string Update(string user)
			{
				return $"{user} isimli kullanıcı başarıyla güncellenmiştir";
			}
		}
	}
}
